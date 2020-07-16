using CharacterManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Contracts.Repositories
{
    public interface ICharacterRepository
    {
        Task<Character> LoadByCharacterName(string characterName);

        Task SaveCharacter(Character character);

    }
}
