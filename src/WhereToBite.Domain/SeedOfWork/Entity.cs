using System.Diagnostics.CodeAnalysis;

namespace WhereToBite.Domain.SeedOfWork
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private int _id;        
        public virtual int Id 
        {
            get => _id;
            protected set => _id = value;
        }

        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            
            return item.Id == Id;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            if (IsTransient())
            {
                // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
                return base.GetHashCode();
            }
            
            // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
            _requestedHashCode ??= Id.GetHashCode() ^ 31;

            return _requestedHashCode.Value;

        }
        public static bool operator ==(Entity left, Entity right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}