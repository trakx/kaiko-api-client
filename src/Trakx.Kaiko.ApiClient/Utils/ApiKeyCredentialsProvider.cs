using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Trakx.Utils.Apis;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Kaiko.ApiClient.Utils;

public interface IKaikoCredentialsProvider : ICredentialsProvider {}
public class ApiKeyCredentialsProvider : IKaikoCredentialsProvider, IDisposable
{
    private const string ApiKeyHeader = "X-Api-Key";

    private readonly KaikoApiConfiguration _configuration;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CancellationTokenSource _tokenSource;

    private static readonly ILogger Logger = Log.Logger.ForContext<ApiKeyCredentialsProvider>();

    public ApiKeyCredentialsProvider(KaikoApiConfiguration configuration, IDateTimeProvider dateTimeProvider)
    {
        _configuration = configuration;
        _dateTimeProvider = dateTimeProvider;

        _tokenSource = new CancellationTokenSource();
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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _tokenSource.Cancel();
        _tokenSource?.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}