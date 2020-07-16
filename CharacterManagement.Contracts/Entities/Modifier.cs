using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public struct Modifier
    {
        public string AffectedObject { get; set; }
        public string AffectedValue { get; set; }
        public int Value { get; set; }
    }
}
