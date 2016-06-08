using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace GradesSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("students");
            var collection = db.GetCollection<Score>("grades");

            var filter = new BsonDocument("type", "homework");

            var list = await collection
                .Find(filter)
                .Sort(Builders<Score>.Sort.Ascending("student_id").Ascending("score"))
                .ToListAsync();

            int? previousId = null;
                  
            foreach (var doc in list)
            {
                if (previousId != doc.student_id)
                {
                    await collection.FindOneAndDeleteAsync(x => x._id == doc._id);                    
                    previousId = doc.student_id;
                }
            }
        }
    }

    class Score
    {
        public ObjectId _id { get; set; }
        public int student_id { get; set; }
        public string type { get; set; }
        public double score { get; set; }
    }

}
