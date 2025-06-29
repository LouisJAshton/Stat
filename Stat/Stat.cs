namespace LouisAshton.Stat
{
    public class Stat<T>
    {
        public readonly T InitialValue;
        public T Value { get; private set; }
        public Stat(T initialValue) => Value = InitialValue = initialValue;
        
        //Can be implicitly cast to T to access the stored value
        public static implicit operator T(Stat<T> t) => t.Value;
        
        //Can be implicitly cast to ReadOnlyStat<T> to be used as the backing field for a ReadOnlyStat property
        //This allows outside classes to read the value and listen to changes without modifying it
        public static implicit operator ReadOnlyStat<T>(Stat<T> stat) => new (stat);

        /// <summary>
        /// Assignment that invokes the onStatChanged delegates
        /// </summary>
        /// <param name="newValue">The new value to set</param>
        public void Set(T newValue)
        {
            var oldValue = Value;
            Value = newValue;
            _onStatChanged?.Invoke(oldValue, newValue);
            _onStatChangedNoParams?.Invoke();
        }

        /// <summary>
        /// Reset the value to the initial value
        /// </summary>
        public void Reset() => Set(InitialValue);
        
        //Callback management
        
        //Callback with old/new parameters
        public delegate void StatChangedCallback(T oldValue, T newValue);
        private StatChangedCallback _onStatChanged;
        public void Listen(StatChangedCallback callback)
        {
            _onStatChanged -= callback;
            _onStatChanged += callback;
        }
        public void StopListen(StatChangedCallback callback)
            => _onStatChanged -= callback;
        
        //Callback without parameters
        public delegate void StatChangedCallbackNoParams();
        private StatChangedCallbackNoParams _onStatChangedNoParams;
        public void Listen(StatChangedCallbackNoParams callback)
        {
            _onStatChangedNoParams -= callback;
            _onStatChangedNoParams += callback;
        }
        public void StopListen(StatChangedCallbackNoParams callback)
            => _onStatChangedNoParams -= callback;
        
    }

    /// <summary>
    /// ReadOnly struct to access a Stat without providing external Set access whilst still allowing Listen and Get access 
    /// </summary>
    public readonly struct ReadOnlyStat<T>
    {
        private readonly Stat<T> _stat;
        public T InitialValue => _stat.InitialValue;
        public T Value => _stat.Value;
        
        public ReadOnlyStat(Stat<T> stat) => _stat = stat;
        
        //Can be implicitly cast to T to access the stored value
        public static implicit operator T(ReadOnlyStat<T> t) => t.Value;
        
        //Allows access to 
        public void Listen(Stat<T>.StatChangedCallback callback) => _stat.Listen(callback);
        public void StopListen(Stat<T>.StatChangedCallback callback) => _stat.StopListen(callback);
        public void Listen(Stat<T>.StatChangedCallbackNoParams callback) => _stat.Listen(callback);
        public void StopListen(Stat<T>.StatChangedCallbackNoParams callback) => _stat.StopListen(callback);
    }
}