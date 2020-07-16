using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public struct Class
    {
        public string Name { get; set; }
        public int HitDiceValue { get; set; }
        public int ClassLevel { get; set; }
    }
}
