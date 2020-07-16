using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public class CharacterHealth
    {        
        public int Max { get; set; }
        public int Temp { get; set; }
        public int Current { get; set; }
        public int Total { 
            get
            {
                return Current + Temp;
            }
        }
    }
}
