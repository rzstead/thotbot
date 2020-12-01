using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Controllers
{
    public class LeaderboardController : Leaderboard.LeaderboardBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        public override async Task<LeaderboardResponse> Top(LeaderboardRequest request, ServerCallContext context)
        {
            var response = new LeaderboardResponse();
            var serverId = request.ServerId;
            var authorId = request.AuthorId;
            var pagesToSkip = request.PagesToSkip;

            var wordSet = await _leaderboardService.Top(serverId, authorId, pagesToSkip);
            var words = wordSet.OrderByDescending(x => x.SeenTotal).ToList();

            if (authorId == 0)
            {
                foreach (var word in words)
                {
                    if (word.SeenFrom.Any())
                    {
                        var user = word.SeenFrom.OrderByDescending(x => x.Amount).First();

                        response.WordCount.Add(new UserWordCount { AuthorId = user.UserId, Count = user.Amount, Word = word.Value });
                    }
                }
            }
            else
            {
                foreach (var word in words)
                {
                    if (word.SeenFrom.Any())
                    {
                        var user = word.SeenFrom.OrderByDescending(x => x.Amount).FirstOrDefault(x => x.UserId == authorId);
                        if (user is null) continue;

                        response.WordCount.Add(new UserWordCount { AuthorId = user.UserId, Count = user.Amount, Word = word.Value });
                    }
                }
            }
            return response;
        }
    }
}