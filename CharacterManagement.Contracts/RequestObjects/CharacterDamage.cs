using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.RequestObjects
{
    public struct CharacterDamage
    {
        public int Amount { get; set; }
        public string Type { get; set; }
    }
}
