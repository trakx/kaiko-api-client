using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace Trakx.Kaiko.ApiClient.Stream;

public class ObservableStream<T> : IObservable<T>
{
    private readonly IAsyncStreamReader<T> _reader;
    private readonly CancellationToken _token;

    public ObservableStream(IAsyncStreamReader<T> reader, CancellationToken token = default)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _token = token;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        var canSubscribe = CanSubscribeOnce();
        if (!canSubscribe) throw new InvalidOperationException("Subscribe can only be called once.");
        return new StreamSubscription<T>(_reader, observer, _token);
    }

    #region Subscribe once

    private int _used;

    private bool CanSubscribeOnce() => Interlocked.Exchange(ref _used, 1) == 0;

    #endregion
}
