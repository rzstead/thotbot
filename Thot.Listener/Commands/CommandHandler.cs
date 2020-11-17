using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Thot.Listener.Util;
using Microsoft.Extensions.DependencyInjection;
using Discord;

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
            _discordClient.MessageReceived += HandleCommandAsync;
            _commands.CommandExecuted += CommandExecuted;
        }

        private async Task CommandExecuted(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!info.IsSpecified)
            {
                await context.Channel.SendMessageAsync("That command doesn't exist. Try using !thot.help.");
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
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