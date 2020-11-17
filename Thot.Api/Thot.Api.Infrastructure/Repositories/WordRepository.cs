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
        public WordRepository(IWordDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _words = database.GetCollection<WordSet>(settings.WordCollectionName);
        }

        public async Task<WordSet> Get(ulong guildId)
        {
            var word = _words.AsQueryable().FirstOrDefault();
            System.Console.WriteLine($"Found word: {word}");
            return await _words.Find<WordSet>(word => word.ServerId == guildId).FirstOrDefaultAsync();
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
                var existingWords = existingSet.Words.Select(x => x.Value);
                var duplicateWords = toAdd.Intersect(existingWords).ToList();

                var additionalSet = toAdd.Where(wordToAdd => !existingWords.Contains(wordToAdd))
                .Select(x => new Word() { Value = x });

                existingSet.Words.AddRange(additionalSet);

                await Update(guildId, existingSet.Words);

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

            var nonExistentWords = toDelete.Where(word => !existingSet.Words.Select(x => x.Value).Contains(word)).ToList();
            System.Console.WriteLine(string.Join(',', nonExistentWords));
            foreach (var word in toDelete)
            {
                var wordToRemove = existingSet.Words.Find(x => x.Value == word);
                existingSet.Words.Remove(wordToRemove);
            }

            await Update(guildId, existingSet.Words);

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

            if (existingSet is null || !existingSet.Words.Any())
            {
                return "No words tracked to be reset.";
            }

            existingSet.Words.ForEach(word =>
            {
                word.SeenFrom = new List<SeenFrom>();
                word.SeenTotal = 0;
            });

            await Update(guildId, existingSet.Words);
            return "";
        }
    }
}