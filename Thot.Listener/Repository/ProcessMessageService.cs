using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Thot.Listener;

public class ProcessMessageService
{
    private readonly MessageProcessor.MessageProcessorClient _client;
    public ProcessMessageService()
    {
        var httpHandler = new HttpClientHandler();
        // Return `true` to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
        _client = new MessageProcessor.MessageProcessorClient(channel);
    }

    public void ProcessMessage(ulong authorId, ulong serverId, string message)
    {
        var request = new ProcessRequest()
        {
            ServerId = serverId,
            AuthorId = authorId,
            Message = message
        };

        _ = Task.Run(() => _client.ProcessAsync(request));
    }
}