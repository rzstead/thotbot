using System.Threading.Tasks;

namespace Thot.Api.Core.Interfaces
{
    public interface IProcessorService
    {
        Task Process(ulong serverId, ulong authorId, string content);
    }
}