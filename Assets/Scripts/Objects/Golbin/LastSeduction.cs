using System;
using Entities.Types;

namespace Objects.Golbin
{
    [Serializable]
    public class LastSeduction
    {
        public SeductionType? type = null;
        public int count = 0;

        public LastSeduction(SeductionType? type = null, int count = 0)
        {
            this.type = type;
            this.count = count;
        }

        public void Update(SeductionType newType)
        {
            if (type == newType)
            {
                count++;
            }
            else
            {
                type = newType;
                count = 0;
            }
        }
    }
}