using System;

namespace Data
{
    [Serializable]
    public class PlayerAbilities
    {
        public bool attack = true;
        public bool startDate = true;
        public bool die = true;

        // Dating
        public bool changeActive = true;
        public bool compendium = true;
        public bool compliment = true;
        public bool flirt = true;
        public bool insult = true;
        public bool present = true;
        public bool ask = true;
    }
}