using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public interface IKaikoApiCredentialsProvider : ICredentialsProvider { }

public class ApiKeyCredentialsProvider : IKaikoApiCredentialsProvider
{
    private const string ApiKeyHeader = "X-Api-Key";

    private readonly KaikoApiConfiguration _configuration;

    private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

    public ApiKeyCredentialsProvider(KaikoApiConfiguration configuration)
    {
        _configuration = configuration;
    }

    #region Implementation of ICredentialsProvider

    /// <inheritdoc />
    public void AddCredentials(HttpRequestMessage msg)
    {
        msg.Headers.Add(ApiKeyHeader, _configuration.ApiKey);
        Logger.Verbose($"{ApiKeyHeader} Header added");
    }

    public Task AddCredentialsAsync(HttpRequestMessage msg)
    {
        AddCredentials(msg);
        return Task.CompletedTask;
    }

    #endregion
}
