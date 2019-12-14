using CosmosDB.SQLRepo.Contract;
using System;
using System.Text;

namespace CosmosDB.SQLRepo
{
    public class SqlConfig : ISqlConfig
    {
        static class EnvironmentNames
        {
            public const string EndPointUri = "EndPointUri";
            public const string PrimaryKey = "PrimaryKey";
            public const string Database = "Database";
            public const string Container = "Container";
            public const string PartitionKey = "PartitionKey";
        }

        public SqlConfig()
        {
            EndPointUri = GetEndPointUri();
            PrimaryKey = GetPrimaryKey();
            DataBase = GetDatabase();
            Container = GetContainer();
            PartitionKey = GetPartitionKey();

        }
        public SqlConfig(string endPointUrl, string primaryKey, string database, string container, string partitionKey)
        {
            EndPointUri = endPointUrl;
            PrimaryKey = primaryKey;
            DataBase = database;
            Container = container;
            PartitionKey = partitionKey;
        }

        public static string GetEndPointUri()
        {
            var val = Environment.GetEnvironmentVariable(EnvironmentNames.EndPointUri);

            val = string.IsNullOrEmpty(val) ? string.Empty : val;

            return val;
        }

        public static string GetPrimaryKey()
        {
            var val = Environment.GetEnvironmentVariable(EnvironmentNames.PrimaryKey);

            val = string.IsNullOrEmpty(val) ? string.Empty : val;

            return val;
        }

        public static string GetDatabase()
        {
            var val = Environment.GetEnvironmentVariable(EnvironmentNames.Database);

            val = string.IsNullOrEmpty(val) ? string.Empty : val;

            return val;
        }

        public static string GetContainer()
        {
            var val = Environment.GetEnvironmentVariable(EnvironmentNames.Container);

            val = string.IsNullOrEmpty(val) ? string.Empty : val;

            return val;
        }

        public static string GetPartitionKey()
        {
            var val = Environment.GetEnvironmentVariable(EnvironmentNames.PartitionKey);

            val = string.IsNullOrEmpty(val) ? string.Empty : val;

            return val;
        }

        public string EndPointUri
        {
            get;
            set;
        }

        public string PrimaryKey
        {
            get;
            set;
        }
        public string DataBase
        {
            get;
            set;
        }

        public string Container
        {
            get;
            set;
        }

        public string PartitionKey
        {
            get;
            set;
        }
    }
}
