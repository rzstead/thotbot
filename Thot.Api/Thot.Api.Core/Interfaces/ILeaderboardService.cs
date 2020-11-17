using System.Collections.Generic;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;

namespace Thot.Api.Core.Interfaces
{
    public interface ILeaderboardService
    {
        Task<WordSet> Top(ulong serverId, ulong authorId);
    }
}