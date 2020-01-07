namespace ReceiveEPHClient
{
    public class MySettings
    {
        public string Name { get; set; }
        public string EventHubConnectionString { get; set; }
        public string EventHubName { get; set; }
        public string StorageContainerName { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }

        public string StorageConnectionString => string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);
    }
}
