using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class User
    {
        public ObjectId Id;
        public string Name;
        public string Email;
    }
}