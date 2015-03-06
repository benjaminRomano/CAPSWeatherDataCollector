namespace WeatherDataCollector.StorageProvider
{
    public interface IStorageProvider
    {
        string Add(StorageProviderAddParams addParams);
    }
}
