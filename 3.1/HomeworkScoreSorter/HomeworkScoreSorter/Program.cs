using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using System.Linq;
using System.Threading.Tasks;

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
            var conventionPack = new ConventionPack();
            conventionPack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var client = new MongoClient();
            var db = client.GetDatabase("school");
            var collection = db.GetCollection<Student>("students");

            var list = await collection.FindAsync(new BsonDocument()).Result.ToListAsync();

            foreach (var doc in list)
            {
                Console.WriteLine();
                Console.WriteLine("STUDENT " + doc.Id);

                RemoveLowestHomeWorkScore(doc);

                await UpdateCollection(doc, collection);

                Console.WriteLine(doc.ToBsonDocument());

                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("===============================================================");
            }
        }

        private static void RemoveLowestHomeWorkScore(Student doc)
        {
            var homeworkScores = doc.Scores
                .Where(t => t.Type == "homework");

            var orderedScores = homeworkScores
                .OrderBy(x => x.Score);

            var lowestHomeWorkScore = orderedScores.FirstOrDefault();

            doc.Scores.Remove(lowestHomeWorkScore);
        }

        private static async Task UpdateCollection(Student doc, IMongoCollection<Student> collection)
        {
            await collection.UpdateOneAsync(
                Builders<Student>.Filter.Eq(x => x.Id, doc.Id),
                Builders<Student>.Update.Set(x => x.Scores, doc.Scores));
        }
    }

    public class Student
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Result> Scores { get; set; }
    }

    public class Result
    {
        public string Type { get; set; }
        public double Score { get; set; }
    }
}
