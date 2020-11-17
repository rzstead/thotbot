using System.Collections.Generic;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Core.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepo;
        public WordService(IWordRepository wordRepo)
        {
            _wordRepo = wordRepo;
        }
        public async Task<string> Add(ulong guildId, List<string> toAdd)
        {
            return await _wordRepo.Add(guildId, toAdd);
        }

        public async Task<WordSet> Get(ulong guildId)
        {
            return await _wordRepo.Get(guildId);
        }

        public async Task<string> Remove(ulong guildId, List<string> toDelete)
        {
            return await _wordRepo.Remove(guildId, toDelete);
        }

        public async Task<string> Reset(ulong guildId, List<string> toReset = null)
        {
            return await _wordRepo.Reset(guildId, toReset);
        }

        public async Task Update(ulong guildId, List<Word> toUpdate)
        {
            await _wordRepo.Update(guildId, toUpdate);
        }
    }
}