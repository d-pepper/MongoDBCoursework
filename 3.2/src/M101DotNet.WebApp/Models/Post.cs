using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Post
    {
        // XXX WORK HERE
        // add in the appropriate properties for a post
        // The homework instructions contain the schema.
        public ObjectId Id;
        public string Author;
        public string Title;
        public string Content;
        public IEnumerable<string> Tags;
        public DateTime CreatedAtUtc;
        public IEnumerable<Post> Comments;
    }
}