namespace Trakx.Kaiko.ApiClient;

public interface IReferenceDataClient { }

public abstract class ReferenceDataClient : IReferenceDataClient
{
    protected string BaseUrl { get; }

    protected ReferenceDataClient(ClientConfigurator configurator)
    {
        BaseUrl = configurator.ApiConfiguration.BaseUrl;
    }
}