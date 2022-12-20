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
        while (!token.IsCancellationRequested)
        {
            try
            {
                if (!await _reader.MoveNext(token)) break;
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
            {
                break;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                _observer.OnError(e);
                _completed = true;
                return;
            }

            _observer.OnNext(_reader.Current);
        }

        _completed = true;
        _observer.OnCompleted();
    }

    public void Dispose()
    {
        if (!_completed && !_cancellationSource.IsCancellationRequested)
        {
            _cancellationSource.Cancel();
        }

        _cancellationSource.Dispose();
        _task.Dispose();

        GC.SuppressFinalize(this);
    }
}
