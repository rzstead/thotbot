using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Linq;
using System;
using System.Net.Http;

namespace Thot.Listener.Repository
{
    public class WordService
    {
        private Words.WordsClient _client;

        public WordService()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });
            _client = new Words.WordsClient(channel);
        }

        /// <exception cref="System.Exception">Thrown when something goes wrong</exception>
        public async Task<string> Add(ulong serverId, List<string> toAdd)
        {
            AddRequest request = new AddRequest();
            request.ServerId = serverId;
            request.Words.AddRange(toAdd);
            try
            {
                var reply = await _client.AddAsync(request);
                return reply.Message;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "An error has occured. Please try again.";
            }
        }

        /// <exception cref="System.Exception">Thrown when something goes wrong</exception>
        public async Task<string> Remove(ulong serverId, List<string> toRemove)
        {
            RemoveRequest request = new RemoveRequest();
            request.ServerId = serverId;
            request.Words.AddRange(toRemove);
            try
            {
                var reply = await _client.RemoveAsync(request);
                return reply.Message;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "An error has occured. Please try again.";
            }
        }

        /// <exception cref="System.Exception">Thrown when something goes wrong</exception>
        public async Task<List<WordCount>> List(ulong serverId, int pagesToSkip)
        {
            ListResponse result = new ListResponse();

            result = await _client.ListAsync(new ListRequest { ServerId = serverId, PagesToSkip = pagesToSkip });

            return result.WordCounts.ToList();
        }

        /// <exception cref="System.Exception">Thrown when something goes wrong</exception>
        public async Task<string> Reset(ulong serverId, List<string> wordsToReset = null)
        {
            ResultMessage result = new ResultMessage();
            try
            {
                var request = new ResetRequest { ServerId = serverId };
                request.WordsToReset.AddRange(wordsToReset);

                result = await _client.ResetAsync(request);
                return result.Message;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "An error has occured. Please try again.";
            }
        }
    }
}
