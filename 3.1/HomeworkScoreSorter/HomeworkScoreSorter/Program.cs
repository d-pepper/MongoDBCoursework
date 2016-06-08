using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HomeworkScoreSorter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args);
            Console.ReadLine();
        }

        private static async void MainAsync(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("school");
            var collection = db.GetCollection<Student>("students");

            var list = await collection.FindAsync(new BsonDocument()).Result.ToListAsync();

            foreach (var doc in list)
            {
                Console.WriteLine(doc.ToBsonDocument());
            }
        }
    }

    public class Student
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public List<Score> scores { get; set; }
    }

    public class Score
    {
        public string type { get; set; }
        public float score { get; set; }
    }
}
