namespace CosmosDB.SQLRepo.Contract
{
    public interface ISqlConfig
    {
        string EndPointUri { get; }

        string PrimaryKey { get; }

        string DataBase { get; }

        string Container { get; }

        string PartitionKey { get; }
    }
}
