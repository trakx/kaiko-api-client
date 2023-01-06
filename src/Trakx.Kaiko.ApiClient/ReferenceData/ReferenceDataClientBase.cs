namespace Trakx.Kaiko.ApiClient;

internal abstract class ReferenceDataClientBase : ClientBase
{
    protected ReferenceDataClientBase(ClientConfigurator configurator)
        : base(configurator, configurator.ApiConfiguration.ReferenceDataBaseUrl)
    {
    }
}
