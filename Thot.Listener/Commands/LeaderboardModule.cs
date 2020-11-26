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
                words = await _leaderboardService.TopAsync(user ?? 0, Context.Guild.Id, 0);
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
                    var users = Context.Guild.Users;
                    embed = EmbedFactory.BuildLeaderboardEmbed(words, 0, null, users);
                }
                else
                {
                    embed = EmbedFactory.BuildLeaderboardEmbed(words, 0, userMention, null);
                }

                var message = await ReplyAsync("", false, embed);
                await message.AddReactionsAsync(new IEmote[] {
                    new Emoji(PaginationEmote.Back.EmoteValue),
                    new Emoji(PaginationEmote.Forward.EmoteValue) });
            }
            else
            {
                await ReplyAsync($"I have no data to show you!");
            }
        }
    }
}