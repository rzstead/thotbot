namespace Thot.Api.Core.DTOs
{
    public class WordDatabaseSettings : IWordDatabaseSettings
    {
        public string WordCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IWordDatabaseSettings
    {
        public string WordCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}