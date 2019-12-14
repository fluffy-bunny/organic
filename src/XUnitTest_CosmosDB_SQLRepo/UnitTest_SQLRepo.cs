using CosmosDB.SQLRepo;
using CosmosDB.SQLRepo.Contract;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using XUnitTest_CosmosDB_SQLRepo.models;

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

        static ISqlConfig NewSqlConfig()
        {
            var sqlConfig = SqlConfig.NewEmulatorConfig();
            sqlConfig.DataBase = "Test";
            sqlConfig.Container = "Ratings";
            sqlConfig.PartitionKey = "/productId";

            return sqlConfig;
        }
        static ISqlRepository<T> NewSqlRepository<T>(ISqlConfig config)
        {
            return new SQLRepository<T>(config);
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
    }
}
