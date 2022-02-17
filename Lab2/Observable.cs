using System;
using System.Collections.Generic;

class Observable<T> where T: class
{
    private T _observedObject;
    private Dictionary<UnsubscribeHandle<T>, Action<T>> _subscribers;

    public Observable(T observedObject)
    {
        this._observedObject = observedObject;
        this._subscribers = new Dictionary<UnsubscribeHandle<T>, Action<T>>();
    }

    public IDisposable Subscribe(Action<T> onNext, bool ASAP = false)
    {
        if (onNext == null) return null;
        var result = new UnsubscribeHandle<T>(this);
        this._subscribers.Add(result, onNext);
        if (ASAP) onNext(this._observedObject);
        return result;
    }

    public void Unsubscribe(UnsubscribeHandle<T> handle)
    {
        this._subscribers.Remove(handle);
    }

    public Observable<T> Use(Action<T> action)
    {
        if (action != null) { 
            action(this._observedObject);
            this.Broadcast();
        } 
        return this;
    }

    private void Broadcast()
    {
        foreach (var kvPair in this._subscribers) {
            kvPair.Value(this._observedObject);
        }
    }
}

class UnsubscribeHandle<T>: IDisposable where T: class
{
    private Observable<T> _observable;

    public UnsubscribeHandle(Observable<T> observable)
    {
        this._observable = observable;
    }

    public void Dispose()
    {
        if (this._observable == null) return;
        this._observable.Unsubscribe(this);
    }
}
