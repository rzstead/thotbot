using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Thot.Api.Core.DTOs;
using Thot.Api.Core.Entities;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Infrastructure.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly IMongoCollection<WordSet> _words;
        private const int ROWS_PER_PAGE = 9;
        public WordRepository(IWordDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _words = database.GetCollection<WordSet>(settings.WordCollectionName);
        }

        public async Task<List<Word>> Get(ulong guildId, ulong authorId = 0, int pagesToSkip = 0)
        {
            var serverWordSet = await _words.Find(x => x.ServerId == guildId).FirstOrDefaultAsync();
            var localWordSet = serverWordSet.Words;

            if (serverWordSet is null) return new List<Word>();

            if (authorId > 0)
            {
                localWordSet = serverWordSet.Words.Where(x => x.SeenFrom.Any(x => x.UserId == authorId)).ToList();
            }

            localWordSet = localWordSet.OrderByDescending(x => x.SeenTotal).Skip(pagesToSkip * ROWS_PER_PAGE).Take(ROWS_PER_PAGE).ToList();
            return localWordSet;
        }

        public async Task<string> Add(ulong guildId, List<string> toAdd)
        {
            var existingSet = await Get(guildId);

            if (existingSet is null)
            {
                var newSet = toAdd.Select(x => new Word() { Value = x });
                await _words.InsertOneAsync(new WordSet()
                {
                    ServerId = guildId,
                    Words = newSet.ToList(),
                });
            }
            else
            {
                var existingWords = existingSet.Select(x => x.Value);
                var duplicateWords = toAdd.Intersect(existingWords).ToList();

                var additionalSet = toAdd.Where(wordToAdd => !existingWords.Contains(wordToAdd))
                .Select(x => new Word() { Value = x });

                existingSet.AddRange(additionalSet);

                await Update(guildId, existingSet);

                if (duplicateWords.Any())
                {
                    return $"Skipped adding {string.Join(", ", duplicateWords)} - already added.";
                }
            }
            return "";
        }

        public async Task<string> Remove(ulong guildId, List<string> toDelete)
        {
            var existingSet = await Get(guildId);

            if (existingSet is null)
            {
                return "There's nothing to delete.";
            }

            var nonExistentWords = toDelete.Where(word => !existingSet.Select(x => x.Value).Contains(word)).ToList();
            System.Console.WriteLine(string.Join(',', nonExistentWords));
            foreach (var word in toDelete)
            {
                var wordToRemove = existingSet.Find(x => x.Value == word);
                existingSet.Remove(wordToRemove);
            }

            await Update(guildId, existingSet);

            if (nonExistentWords.Any())
            {
                var messagePrefix = $"Skipped removing {string.Join(", ", nonExistentWords)} - ";
                var message = messagePrefix + (nonExistentWords.Count() > 1 ? "they aren't tracked." : "it isn't tracked.");
                return message;
            }

            return "";
        }

        public async Task Update(ulong guildId, List<Word> toUpdate)
        {
            var update = Builders<WordSet>.Update.Set(x => x.Words, toUpdate);

            await _words.UpdateOneAsync(words => words.ServerId == guildId, update);
        }

        public async Task<string> Reset(ulong guildId, List<string> toReset = null)
        {
            var existingSet = await Get(guildId);

            if (existingSet is null || !existingSet.Any())
            {
                return "No words tracked to be reset.";
            }

            existingSet.ForEach(word =>
            {
                word.SeenFrom = new List<SeenFrom>();
                word.SeenTotal = 0;
            });

            await Update(guildId, existingSet);
            return "";
        }
    }
}