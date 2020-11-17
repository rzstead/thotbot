using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Thot.Api.Core.Entities
{
    public class WordSet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("serverId")]
        public ulong ServerId { get; set; }
        [BsonElement("wordSet")]
        public List<Word> Words { get; set; } = new List<Word>();
    }

    public class Word
    {
        [BsonElement("word")]
        public string Value { get; set; }
        [BsonElement("seenTotal")]
        public int SeenTotal { get; set; }
        [BsonElement("seenFrom")]
        public List<SeenFrom> SeenFrom { get; set; } = new List<SeenFrom>();

    }

    public class SeenFrom
    {
        [BsonElement("userId")]
        public ulong UserId { get; set; }
        [BsonElement("amount")]
        public int Amount { get; set; }
    }
}