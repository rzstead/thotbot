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
        private const int PAGE_LIMIT = 9;
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
            var userId = Context.Message.MentionedUsers.FirstOrDefault(x => x.Mention == userMention)?.Id;
            var users = Context.Guild.Users;
            var words = new List<UserWordCount>();
            try
            {
                words = await _leaderboardService.TopAsync(userId ?? 0, Context.Guild.Id, 0);
            }
            catch (Exception e)
            {
                System.Console.Error.Write(e);
            }

            if (words.Any())
            {
                Embed embed = null;
                if (userMention is null)
                {
                    embed = EmbedFactory.BuildLeaderboardEmbed(words, 0, null, users);
                }
                else
                {
                    var user = users.FirstOrDefault(x => x.Mention == userMention);
                    embed = EmbedFactory.BuildLeaderboardEmbed(words, 0, user, null);
                }

                var message = await ReplyAsync("", false, embed);
                if (words.Count() == 9)
                {
                    await message.AddReactionsAsync(new IEmote[] {
                    new Emoji(PaginationEmote.Forward.EmoteValue) });
                }
            }
            else
            {
                await ReplyAsync($"I have no data to show you!");
            }
        }
    }
}