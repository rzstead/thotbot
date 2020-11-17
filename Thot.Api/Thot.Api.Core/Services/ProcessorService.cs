using System.Linq;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Core.Services
{
    public class ProcessorService : IProcessorService
    {
        private readonly IWordService _wordService;
        public ProcessorService(IWordService wordService)
        {
            _wordService = wordService;
        }
        public async Task Process(ulong serverId, ulong authorId, string content)
        {
            var dbWordSet = await _wordService.Get(serverId);

            if (dbWordSet is null) return;

            var dbWords = dbWordSet.Words.Select(x => x.Value);
            var incomingWords = content.Split(' ').ToList();

            foreach (var incomingWord in incomingWords)
            {
                //determine if word is being tracked at all
                var dbWord = dbWordSet.Words.FirstOrDefault(x => x.Value == incomingWord);
                if (dbWord is null) continue;

                //if it is tracked, attempt to create or update the seenFrom values.
                var seenFrom = dbWord.SeenFrom.FirstOrDefault(x => x.UserId == authorId);
                if (seenFrom is null)
                {
                    seenFrom = new SeenFrom { Amount = 0, UserId = authorId };
                    dbWord.SeenFrom.Add(seenFrom);
                }

                seenFrom.Amount++;
                dbWord.SeenTotal++;
            }

            await _wordService.Update(serverId, dbWordSet.Words);
            return;
        }
    }
}