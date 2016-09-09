using System;

namespace Tagolog.Private.Correlation
{
    abstract class ThreadCorrelationId
    {
        const string _emptyBaseId = "";
        const string _partSeparator = "_";

        int _curPartId;

        public int GetNextPartId()
        {
            return ++_curPartId;
        }

        public string BaseId { get; protected set; }
        public int PartId { get; protected set; }

        protected virtual string EmptyBaseId
        {
            get { return _emptyBaseId; }
        }

        protected virtual string PartSeparator
        {
            get { return _partSeparator; }
        }

        protected ThreadCorrelationId() : this( null )
        {
        }

        protected ThreadCorrelationId( ThreadCorrelationId parent )
        {
            if ( parent == null )
            {
                BaseId = GenerateNewBaseId();
            }
            else
            {
                BaseId = parent.ToString();
                PartId = parent.GetNextPartId();
            }
        }

        protected abstract string GenerateNewBaseId();

        #region Overrides of Object

        public override string ToString()
        {
            if ( EmptyBaseId.Equals( BaseId ) )
            {
                return EmptyBaseId;
            }
            return string.Format( "{0}{1}{2:D3}", BaseId, PartSeparator, PartId );
        }

        public override bool Equals( object obj )
        {
            if ( ReferenceEquals( null, obj ) )
            {
                return false;
            }
            if ( ReferenceEquals( this, obj ) )
            {
                return true;
            }
            if ( obj.GetType() != GetType() )
            {
                return false;
            }
            return ToString() == obj.ToString();
        }

        #endregion

        public static bool Equals( ThreadCorrelationId id1, ThreadCorrelationId id2 )
        {
            return ( object.Equals( id1, id2 ) || ( !ReferenceEquals( null, id1 ) && id1.Equals( id2 ) ) );
        }

        public static bool operator ==( ThreadCorrelationId id1, ThreadCorrelationId id2 )
        {
            return Equals( id1, id2 );
        }

        public static bool operator !=( ThreadCorrelationId id1, ThreadCorrelationId id2 )
        {
            return !Equals( id1, id2 );
        }

        public bool Equals( ThreadCorrelationId other )
        {
            if ( ReferenceEquals( null, other ) )
            {
                return false;
            }
            if ( ReferenceEquals( this, other ) )
            {
                return true;
            }
            return Equals( other.BaseId, BaseId ) && other.PartId == PartId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ( ( BaseId != null ? BaseId.GetHashCode() : 0 )*397 ) ^ PartId;
            }
        }

        static readonly string[] _partSeparators = {_partSeparator};

        public static string ExtractBaseCorrelation( string correlationId, out int depth )
        {
            depth = 0;
            if ( string.IsNullOrEmpty( correlationId ) )
            {
                return _emptyBaseId;
            }
            string[] parts = correlationId.Trim().Split( _partSeparators, StringSplitOptions.RemoveEmptyEntries );
            int d = -1;
            string baseCorId = parts[ 0 ].Trim();
            foreach ( string part in parts )
            {
                if ( part.Trim().Length > 0 )
                {
                    ++d;
                }
            }
            depth = Math.Max( 0, d );
            return baseCorId;
        }

        public static string ExtractParentCorrelation( string correlationId )
        {
            if ( string.IsNullOrEmpty( correlationId ) )
            {
                return _emptyBaseId;
            }
            int idx = correlationId.LastIndexOf( _partSeparator );
            if ( idx > 0 )
            {
                return correlationId.Substring( 0, idx );
            }
            return _emptyBaseId;
        }
    }
}
