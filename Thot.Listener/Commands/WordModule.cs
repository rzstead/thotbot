using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Thot.Listener.Repository;

namespace Thot.Listener.Commands
{
    [Group("words")]
    public class WordsModule : ModuleBase<SocketCommandContext>
    {
        private WordService _wordService = new WordService();
        private const int UPPER_WORD_OPERATION_LIMIT = 10;
        private const int UPPER_WORD_LENGTH_LIMIT = 255;

        [Command("add")]
        [Summary("Add some words for me to track")]
        public async Task AddAsync([Summary("The word(s) to start tracking")] params string[] words)
        {
            if (await ValidateWords(words))
            {
                var response = "";
                try
                {
                    response = await _wordService.Add(Context.Guild.Id, words.Select(x => x.ToLower()).ToList());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                    await ErrorMessage();
                }

                if (string.IsNullOrEmpty(response))
                {
                    await ReplyAsync("Added.");
                }
                else
                {
                    await ReplyAsync(response);
                }
            }
        }

        [Command("remove")]
        [Summary("Remove some words from my track list")]
        public async Task RemoveAsync([Summary("The word(s) to remove from my list")] params string[] words)
        {
            if (await ValidateWords(words))
            {
                string response = "";
                try
                {
                    response = await _wordService.Remove(Context.Guild.Id, words.Select(x => x.ToLower()).ToList());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                    await ErrorMessage();
                }

                if (string.IsNullOrEmpty(response))
                {
                    await ReplyAsync("Removed.");
                }
                else
                {
                    await ReplyAsync(response);
                }
            }
        }

        [Command("list")]
        [Summary("List the words I'm tracking")]
        public async Task ListAsync()
        {
            var words = new List<WordCount>();
            try
            {
                words = await _wordService.List(Context.Guild.Id, 0);
            }
            catch (Exception e)
            {
                System.Console.Error.Write(e);
                await ErrorMessage();
            }

            if (words.Any())
            {
                var embed = EmbedFactory.BuildWordListEmbed(words, 0);

                var message = await ReplyAsync("", false, embed);
                await message.AddReactionsAsync(new IEmote[] {
                    new Emoji(PaginationEmote.Back.EmoteValue),
                    new Emoji(PaginationEmote.Forward.EmoteValue) });
            }
            else
            {
                await ReplyAsync($"Add some words for me to track first!");
            }
        }

        [Command("reset")]
        [Summary("Reset my tracking statistics")]
        public async Task ResetAsync(params string[] wordsToReset)
        {
            if (!wordsToReset.Any())
            {
                string response = "";

                try
                {
                    response = await _wordService.Reset(Context.Guild.Id, wordsToReset.Select(x => x.ToLower()).ToList());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                    await ErrorMessage();
                }

                if (string.IsNullOrEmpty(response))
                {
                    await ReplyAsync("Reset all tracking statistics.");
                }
                else
                {
                    await ReplyAsync(response);
                }
            }
            else
            {
                await ReplyAsync("Resetting individual word statistics is currently unsupported.");
            }
        }

        private async Task<bool> ValidateWords(string[] words)
        {
            if (!words.Any())
            {
                await ReplyAsync("I need some words to work with!");
                return false;
            }

            if (words.Count() >= UPPER_WORD_OPERATION_LIMIT)
            {
                await ReplyAsync("Please keep your requests to sets of 10!");
                return false;
            }

            if (words.Any(x => x.Count() > UPPER_WORD_LENGTH_LIMIT))
            {
                await ReplyAsync("Please keep the your word length to less that 255 characters!");
                return false;
            }

            return true;
        }

        private async Task ErrorMessage()
        {
            await ReplyAsync("An error has occured. Please try again.");
        }
    }
}