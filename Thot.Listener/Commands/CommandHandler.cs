using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Thot.Listener.Util;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using System.Linq;
using Discord.Rest;
using System.Collections.Generic;
using Thot.Listener.Repository;

namespace Thot.Listener.Commands
{
    public class Initialize
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly Config _config;

        public Initialize(DiscordSocketClient discordClient = null, CommandService commands = null, Config config = null)
        {
            _commands = commands ?? new CommandService();
            _discordClient = discordClient ?? new DiscordSocketClient();
            _config = config ?? new Config();
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_discordClient)
            .AddSingleton(_commands)
            .AddSingleton(_config)
            .AddSingleton<CommandHandler>()
            .BuildServiceProvider();
    }

    public class CommandHandler
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly Config _config;
        private readonly List<string> PaginatedCommands = new List<string> { "Leaderboard", "Tracked Words" };
        private WordService _wordService = new WordService();
        private LeaderboardService _leaderboardService = new LeaderboardService();


        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services, Config config)
        {
            _commands = commands;
            _discordClient = client;
            _services = services;
            _config = config;
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
            _discordClient.MessageReceived += HandleMessageReceived;
            _discordClient.ReactionAdded += HandleReaction;
            _commands.CommandExecuted += HandleCommandExecuted;
        }

        private async Task HandleReaction(Cacheable<IUserMessage, ulong> messageParam, ISocketMessageChannel channelParam, SocketReaction reaction)
        {
            RestUserMessage message = null;
            if (messageParam.Value is null)
            {
                message = await channelParam.GetMessageAsync(messageParam.Id) as RestUserMessage;
                if (message is null) return;
            }

            var channel = channelParam as SocketGuildChannel;
            if (channel is null) return;

            var authorId = message.Author.Id;
            var serverId = channel.Guild.Id;

            if (authorId == _discordClient.CurrentUser.Id
                && reaction.UserId != _discordClient.CurrentUser.Id
                && message.Embeds.FirstOrDefault(x => PaginatedCommands.Contains(x.Title)) != null)
            {
                var emote = reaction.Emote.Name;
                var page = int.Parse(message.Embeds.First(x => x.Footer.HasValue && x.Footer.Value.Text.Contains("Page")).Footer.Value.Text.Substring(4)) - 1;
                var title = message.Embeds.First().Title;

                if (page >= 0 && PaginationEmote.Emotes.Any(x => x.EmoteValue == emote))
                {
                    Embed embed = null;
                    if (emote == PaginationEmote.Forward.EmoteValue)
                    {
                        page++;
                    }
                    else if (emote == PaginationEmote.Back.EmoteValue)
                    {
                        page--;
                    }

                    switch (title)
                    {
                        case "Leaderboard":
                            var userWords = await _leaderboardService.TopAsync(authorId, serverId, page);
                            var user = message.MentionedUsers.FirstOrDefault()?.Id ?? 0;
                            var users = channel.Guild.Users;
                            embed = EmbedFactory.BuildLeaderboardEmbed(userWords, page, users.FirstOrDefault(x => x.Id == user), users);
                            break;
                        case "Tracked Words":
                            var words = await _wordService.List(serverId, page);
                            embed = EmbedFactory.BuildWordListEmbed(words, page);
                            break;
                    }

                    await message.ModifyAsync((properties) =>
                     {
                         properties.Embed = embed;
                     });

                    await message.RemoveAllReactionsAsync();

                    _ = Task.Run(() =>
                     {
                         message.AddReactionsAsync(new IEmote[] {
                            new Emoji(PaginationEmote.Back.EmoteValue),
                            new Emoji(PaginationEmote.Forward.EmoteValue)
                        });
                     });
                }
            }
        }

        private async Task HandleCommandExecuted(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!info.IsSpecified)
            {
                await context.Channel.SendMessageAsync("That command doesn't exist. Try using !thot.help.");
            }
        }

        private async Task HandleMessageReceived(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            var channel = message.Channel as SocketGuildChannel;
            if (channel is null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            if (message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos) || message.Author.IsBot)
                return;

            if (!(message.HasStringPrefix("!thot.", ref argPos)))
            {
                var authorId = message.Author.Id;
                var processor = new ProcessMessageService();
                try
                {
                    processor.ProcessMessage(authorId, channel.Guild.Id, message.Content.ToLower());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                }
                return;
            }

            var context = new SocketCommandContext(_discordClient, message);
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}