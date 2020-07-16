using CharacterManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Contracts.Services
{
    public interface ICharacterLoadService
    {
        Task<Character> LoadCharacterByName(string characterName);
    }
}
