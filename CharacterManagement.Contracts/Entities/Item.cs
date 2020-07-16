using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public class Item
    {
        public string Name { get; set; }
        public Modifier Modifier { get; set; }
    }
}
