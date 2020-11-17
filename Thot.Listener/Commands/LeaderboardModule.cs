using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using System.Linq;
using Discord;
using System;

namespace Thot.Listener.Commands
{
    public class LeaderboardModule : ModuleBase<SocketCommandContext>
    {
        private LeaderboardService _leaderboardService = new LeaderboardService();

        private CommandService _commandService;
        public LeaderboardModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("top")]
        [Summary("Displays the users who have said the most tracked words. (Can also show per user)")]
        public async Task TopAsync(string userMention = null)
        {
            var user = Context.Message.MentionedUsers.FirstOrDefault(x => x.Username == userMention)?.Id;

            var words = new List<UserWordCount>();
            try
            {
                words = await _leaderboardService.TopAsync(user ?? 0, Context.Guild.Id);
            }
            catch (Exception e)
            {
                System.Console.Error.Write(e);

            }

            if (words.Any())
            {
                var builder = new EmbedBuilder();
                builder.WithTitle("Leaderboard");

                if (userMention is null)
                {
                    var guildUsers = Context.Guild.Users;
                    builder.WithDescription($"And how many times I've seen you all say each");
                    foreach (var word in words)
                    {
                        var wordUser = guildUsers.First(x => x.Id == word.AuthorId).Username;
                        builder.AddField(wordUser, $"{word.Word} - {word.Count}", true);
                    }
                }
                else
                {
                    words.OrderByDescending(x => x.Count).ToList().ForEach(word =>
                    {
                        builder.AddField($"{word.Word}", word.Count, true);
                    });
                    builder.WithDescription($"And how many times I've seen {userMention} say each");
                }

                var embed = builder.Build();

                await ReplyAsync("", false, embed);
            }
            else
            {
                await ReplyAsync($"I have no data to show you!");
            }
        }
    }
}