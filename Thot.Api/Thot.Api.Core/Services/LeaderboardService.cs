using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Core.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IWordService _wordService;
        public LeaderboardService(IWordService wordService)
        {
            _wordService = wordService;
        }

        public async Task<List<Word>> Top(ulong serverId, ulong authorId, int pagesToSkip = 0)
        {
            return await _wordService.Get(serverId, authorId, pagesToSkip);
        }
    }
}