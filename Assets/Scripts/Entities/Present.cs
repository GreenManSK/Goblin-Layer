using System;
using System.Collections.Generic;
using Entities.Types;

namespace Entities
{
    [Serializable]
    public class Present
    {
        public string name;
        public string description;
        public string icon;
        public List<GoblinType> likedBy;

        protected bool Equals(Present other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Present) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }
    }
}