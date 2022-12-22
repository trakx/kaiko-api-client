using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Trakx.Kaiko.ApiClient.Stream;

public class StreamSubscription<T> : IDisposable
{
    private readonly IAsyncStreamReader<T> _reader;
    private readonly IObserver<T> _observer;

    private readonly CancellationTokenSource _cancellationSource;

    private readonly Task _task;

    private bool _completed;

    public StreamSubscription(IAsyncStreamReader<T> reader, IObserver<T> observer, CancellationToken token = default)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));

        _cancellationSource = new CancellationTokenSource();
        token.Register(_cancellationSource.Cancel);

        _task = Run(_cancellationSource.Token);
    }

    private async Task Run(CancellationToken token)
    {
        bool IsStopped() => _completed || token.IsCancellationRequested;

        while (!IsStopped())
        {
            try
            {
                var hasCurrent = await _reader.MoveNext(token).ConfigureAwait(false);
                if (hasCurrent)
                {
                    _observer.OnNext(_reader.Current);
                }
                else
                {
                    _completed = true;
                }
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
            {
                _completed = true;
            }
            catch (OperationCanceledException)
            {
                _completed = true;
            }
            catch (Exception e)
            {
                _observer.OnError(e);
                _completed = true;
                return;
            }
        }

        _observer.OnCompleted();
    }

    #region IDisposable

    private bool _wasDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_wasDisposed) return;
        if (disposing)
        {
            if (!_completed && !_cancellationSource.IsCancellationRequested)
            {
                _cancellationSource.Cancel();
            }
            _cancellationSource.Dispose();
            _task.Dispose();
        }
        _wasDisposed = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
