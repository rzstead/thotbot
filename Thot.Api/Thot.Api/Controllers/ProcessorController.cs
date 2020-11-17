using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Thot.Api.Core.Interfaces;

namespace Thot.Api.Controllers
{
    public class ProcessorController : MessageProcessor.MessageProcessorBase
    {
        private readonly IProcessorService _processorService;
        public ProcessorController(IProcessorService processorService)
        {
            _processorService = processorService;
        }
        public override async Task<Empty> Process(ProcessRequest request, ServerCallContext context)
        {
            await _processorService.Process(request.ServerId, request.AuthorId, request.Message);
            return new Empty();
        }
    }
}