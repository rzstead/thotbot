using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Thot.Listener;

public class LeaderboardService
{
    private readonly Leaderboard.LeaderboardClient _client;
    public LeaderboardService()
    {
        var httpHandler = new HttpClientHandler();
        // Return `true` to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
        _client = new Leaderboard.LeaderboardClient(channel);
    }

    /// <exception cref="System.Exception">Thrown when something goes wrong</exception>
    public async Task<List<UserWordCount>> TopAsync(ulong authorId, ulong serverId, int pagesToSkip)
    {
        var request = new LeaderboardRequest()
        {
            AuthorId = authorId,
            ServerId = serverId,
            PagesToSkip = pagesToSkip
        };

        var response = await _client.TopAsync(request);
        return response.WordCount.ToList();
    }
}