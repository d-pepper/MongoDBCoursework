using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Post
    {
        // XXX WORK HERE
        // add in the appropriate properties for a post
        // The homework instructions contain the schema.
        [BsonId]
        public ObjectId Id;
        [BsonElement("Author")]
        public string Author;
        [BsonElement("Title")]
        public string Title;
        [BsonElement("Content")]
        public string Content;
        [BsonElement("Tags")]
        public IEnumerable<string> Tags;
        [BsonElement("CreateAtUtc")]
        public DateTime CreatedAtUtc;
        [BsonElement("Comments")]
        public IEnumerable<Comment> Comments;
    }
}