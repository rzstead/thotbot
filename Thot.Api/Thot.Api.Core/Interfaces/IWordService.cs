using System.Collections.Generic;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;

namespace Thot.Api.Core.Interfaces
{
    public interface IWordService
    {
        Task<List<Word>> Get(ulong guildId, ulong authorId = 0, int pagesToSkip = 0);
        Task<string> Add(ulong guildId, List<string> toAdd);
        Task<string> Remove(ulong guildId, List<string> toDelete);
        Task Update(ulong guildId, List<Word> toUpdate);
        Task<string> Reset(ulong guildId, List<string> toReset = null);
    }
}