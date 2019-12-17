namespace CosmosDB.SQLRepo.Contract
{
    public interface ISqlConfig<T>
    {
        string EndPointUri { get; }

        string PrimaryKey { get; }

        string DataBase { get; }

        string Container { get; }

        string PartitionKey { get; }
    }
}
