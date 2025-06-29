# Stat

## Overview
```Stat<T>``` is a class that wraps a value of type ```T```. It can be used to allow easy implementation of on-value-changed callbacks in Unity.

## Listening To A Stat

### Listen

- ```public void Listen(StatChangedCallback callback)```
  - Subscribes the passed callback to a delegate called every time the value is updated
  - Unsubscribes the event first to ensure the same delegate is not duplicated
  - Passes both the ```T oldValue``` and the ```T newValue``` as parameters

- ```public void Listen(StatChangedCallbackNoParams callback)```
  - Subscribes the passed callback to a delegate called every time the value is updated
  - Unsubscribes the event first to ensure the same delegate is not duplicated
  - Passes no parameters

### StopListen

- ```public void StopListen(StatChangedCallback callback)```
  - Unsubscribes the given callback

- ```public void StopListen(StatChangedCallbackNoParams callback)```
  - Unsubscribes the given callback

## Use As A Value
- ```public void Reset()```
  - Resets the stored value to the value it was first initialised with

- ```public void Set(T value)```
  - Sets the stored value
  - Invokes the delegates ```_onStatChanged``` and ```_onStatChangedNoParams```

- Can be implicitly cast to type ```T``` to use in place of explicitly calling ```Value```

## ReadOnlyStat<T>
```Stat<T>``` can be cast implicitly to ```ReadOnlyStat<T>``` to protect ```set``` access for its value without restricting access to the ```Listen()``` and ```StopListen()``` methods.
Primarily, it is useful as a Property that is backed by a ```Stat<T>```:
```cs
private Stat<int> _numberStat = new Stat(1);
public ReadOnlyStat<int> NumberStat => _numberStat;
```
