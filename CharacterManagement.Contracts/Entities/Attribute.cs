using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterManagement.Contracts.Entities
{
    public class Attribute
    {
        public Attribute(int score)
        {
            Score = score;
        }
        public int Score { get; set; }
        public int Modifier { 
            get {
                return (int)Math.Floor(((double)Score - 10) / 2);
            }
        }
    }
}
