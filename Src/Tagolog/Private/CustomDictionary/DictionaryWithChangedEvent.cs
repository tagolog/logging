using System;
using System.Collections;
using System.Collections.Generic;

namespace Tagolog.Private.CustomDictionary
{
    class DictionaryWithChangedEvent<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region "IDictionary" interface implementation

        public virtual void Add( TKey key, TValue value )
        {
            _dictionary.Add( key, value );
            OnChanged( key, value );
        }

        public bool ContainsKey( TKey key )
        {
            return _dictionary.ContainsKey( key );
        }

        public bool Remove( TKey key )
        {
            bool result = _dictionary.Remove( key );
            if ( result )
                OnChanged( key );
            return result;
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            return _dictionary.TryGetValue( key, out value );
        }

        public TValue this[ TKey key ]
        {
            get
            {
                return _dictionary[ key ];
            }
            set
            {
                _dictionary[ key ] = value;
                OnChanged( key, value );
            }
        }

        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        #endregion // "IDictionary" interface implementation

        #region "ICollection" interface implementation

        public void Add( KeyValuePair<TKey, TValue> item )
        {
            _dictionary.Add( item );
        }

        public void Clear()
        {
            _dictionary.Clear();
            OnChanged();
        }

        public bool Remove( KeyValuePair<TKey, TValue> item )
        {
            bool result = _dictionary.Remove( item );
            if ( result )
                OnChanged();
            return result;
        }

        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            return _dictionary.Contains( item );
        }

        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            _dictionary.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        #endregion // "ICollection" interface implementation

        #region Enumerators

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion // Enumerators

        public event Action<object, DictionaryChangedEventArgs<TKey,TValue>> Changed;

        #region Helpers

        void OnChanged()
        {
            var changed = Changed;
            if ( null != changed )
                changed.Invoke( this, null );
        }

        void OnChanged( TKey key )
        {
            var changed = Changed;
            if ( null != changed )
                changed.Invoke( this, new DictionaryChangedEventArgs<TKey, TValue>( key ) );
        }

        void OnChanged( TKey key, TValue value )
        {
            var changed = Changed;
            if ( null != changed )
                changed.Invoke( this, new DictionaryChangedEventArgs<TKey, TValue>( key, value ) );
        }

        #endregion // Helpers

        #region Data

        readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        #endregion // Data
    }
}
