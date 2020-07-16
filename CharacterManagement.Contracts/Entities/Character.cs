using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public class Character
    {
        public Character(string name)
        {
            Name = name;
            HP = new CharacterHealth();
            Level = 1;
            Classes = new List<Class>();
            Stats = new Stat();
            Items = new List<Item>();
            Defenses = new List<DefenseValue>();
        }
        public string Name { get; set; }
        public CharacterHealth  HP { get; set; }
        public int Level { get; set; }
        public List<Class> Classes { get; set; }
        public Stat Stats { get; set; }
        public List<Item> Items { get; set; }
        public List<DefenseValue> Defenses { get; set; }

    }
}
