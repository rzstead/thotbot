using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Controllers
{
    public class WordController : Words.WordsBase
    {
        private readonly ILogger<WordController> _logger;

        private readonly IWordService _wordService;
        private const int PAGE_SIZE = 9;

        public WordController(IWordService wordService, ILogger<WordController> logger)
        {
            _logger = logger;
            _wordService = wordService;
        }

        public override async Task<ResultMessage> Add(AddRequest request, ServerCallContext context)
        {
            var message = await _wordService.Add(request.ServerId, request.Words.ToList());
            return new ResultMessage { Message = message };
        }

        public override async Task<ResultMessage> Remove(RemoveRequest request, ServerCallContext context)
        {
            var message = await _wordService.Remove(request.ServerId, request.Words.ToList());
            return new ResultMessage { Message = message };
        }

        public override async Task<ListResponse> List(ListRequest request, ServerCallContext context)
        {
            var pagesToSkip = request.PagesToSkip * PAGE_SIZE;
            var wordSet = await _wordService.Get(request.ServerId);
            var words = wordSet.Words.OrderByDescending(x => x.SeenTotal).Skip(pagesToSkip).Take(PAGE_SIZE).ToList();
            var response = new ListResponse();

            if (wordSet is null)
            {
                return response;
            }

            words.ForEach(word =>
            {
                response.WordCounts.Add(new WordCount { Word = word.Value, Count = word.SeenTotal });
            });

            return response;
        }

        public override async Task<ResultMessage> Reset(ResetRequest request, ServerCallContext context)
        {
            var response = await _wordService.Reset(request.ServerId, request.WordsToReset.ToList());
            return new ResultMessage { Message = response };
        }
    }
}
