using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.Repositories;
using CharacterManagement.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Domain.Services
{
    public class CharacterLoadService : ICharacterLoadService
    {
        private readonly ICharacterRepository _characterRepository;
        public CharacterLoadService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }
        public async Task<Character> LoadCharacterByName(string characterName)
        {
            if (characterName.Length == 0) throw new ArgumentException("Invalid arguments");
            Character character = await _characterRepository.LoadByCharacterName(characterName);
            if (character == null) throw new KeyNotFoundException("Character not found for identifier");
            return character;
        }
    }
}
