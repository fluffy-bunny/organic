using azfun_organics.extensions;
using azfun_organics.models;
using CosmosDB.SQLRepo;
using CosmosDB.SQLRepo.Contract;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest_CosmosDB_SQLRepo
{

    public class UnitTest_SQLRepo
    {
        public UnitTest_SQLRepo()
        {

        }
        static ISqlRepository<Ratings> _ratingsRepo;
        static ISqlRepository<Ratings> RatingsRepo
        {
            get
            {
                if (_ratingsRepo == null)
                {
                    _ratingsRepo = NewSqlRepository<Ratings>(NewSqlConfig());
                }
                return _ratingsRepo;
            }
        }

        static ISqlConfig<Ratings> NewSqlConfig()
        {
            var sqlConfig = SqlConfig<Ratings>.NewEmulatorConfig();
            sqlConfig.DataBase = "Test";
            sqlConfig.Container = "Ratings";
            sqlConfig.PartitionKey = "/productId";

            return sqlConfig;
        }
        static ISqlRepository<T> NewSqlRepository<T>(ISqlConfig<T> config)
        {
            return new SqlRepository<T>(config);
        }

        static string GuidS => Guid.NewGuid().ToString();
        [Fact]
        public async Task Success_WriteRead()
        {

            var rating = new Ratings
            {
                id = GuidS,
                ProductId = "1234",
                LocationName = "Hollywood",
                Rating = 5,
                UserNotes = "Very musky, I love it.",
                Timestamp = DateTime.UtcNow,
                UserId = GuidS

            };


            var result = await RatingsRepo.Insert(rating);

            Expression<Func<Ratings, bool>> lambda = x => x.ProductId == "1234";

            var productRatings = await RatingsRepo.Get(lambda);

            foreach (var item in productRatings)
            {
                Console.WriteLine($"productId:{item.ProductId} Ratings:{item.Rating} Notes:{item.UserNotes}");
            }

        }

        [Fact]
        public void TestDateTimeSerialization()
        {
            DateTime utcNow = DateTime.UtcNow;
            utcNow = utcNow.Truncate(TimeSpan.FromSeconds(1));
            Ratings ratings = new Ratings
            {
                Timestamp = utcNow
            };
            string serialized = JsonConvert.SerializeObject(ratings);
            Ratings deserialized = JsonConvert.DeserializeObject<Ratings>(serialized);
            deserialized.Timestamp.Should().Be(utcNow);
        }
    }
}
