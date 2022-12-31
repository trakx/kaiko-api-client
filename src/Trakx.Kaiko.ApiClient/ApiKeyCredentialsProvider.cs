using Serilog;
using Trakx.Utils.Apis;

namespace Trakx.Kaiko.ApiClient;

public interface IKaikoCredentialsProvider : ICredentialsProvider { }

public class ApiKeyCredentialsProvider : IKaikoCredentialsProvider, IDisposable
{
    private const string ApiKeyHeader = "X-Api-Key";

    private readonly KaikoApiConfiguration _configuration;
    private readonly CancellationTokenSource _cancellationSource;

    private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

    public ApiKeyCredentialsProvider(KaikoApiConfiguration configuration)
    {
        _configuration = configuration;
        _cancellationSource = new CancellationTokenSource();
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


    #region IDisposable

    private bool _wasDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_wasDisposed) return;
        if (disposing)
        {
            if (!_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
            }
            _cancellationSource.Dispose();
        }
        _wasDisposed = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
