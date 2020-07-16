using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public class Stat
    {
        public Stat()
        {
            Strength = new Attribute(8);
            Dexterity = new Attribute(8);
            Constitution = new Attribute(8);
            Intelligence = new Attribute(8);
            Wisdom = new Attribute(8);
            Charisma = new Attribute(8);
        }
        public Attribute Strength { get; set; }
        public Attribute Dexterity { get; set; }
        public Attribute Constitution { get; set; }
        public Attribute Intelligence { get; set; }
        public Attribute Wisdom { get; set; }
        public Attribute Charisma { get; set; }
    }
}
