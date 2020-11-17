using System.Collections.Generic;
using System.Threading.Tasks;
using Thot.Api.Core.Entities;

namespace Thot.Api.Core.Interfaces
{
    public interface IWordService
    {
        Task<WordSet> Get(ulong guildId);
        Task<string> Add(ulong guildId, List<string> toAdd);
        Task<string> Remove(ulong guildId, List<string> toDelete);
        Task Update(ulong guildId, List<Word> toUpdate);
        Task<string> Reset(ulong guildId, List<string> toReset = null);
    }
}