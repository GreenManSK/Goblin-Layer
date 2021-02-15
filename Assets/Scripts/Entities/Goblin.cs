using System;
using System.Collections.Generic;
using Entities.Types;

namespace Entities
{
    [Serializable]
    public class Goblin
    {
        public bool hide = false;
        public Glasses glasses;
        public Blush blush;
        public bool beard;
        public List<Accessory> accessories;
        public Costume costume;
        public HairColor hairColor;
        public HairFront hairFront;
        public HairBack hairBack;
        public Expression expression;
    }
}