using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private static List<Character> _characters = new List<Character>(); //For example purposes i wanted this to be able to run on any DB,
                                                                //so i thought storing it in a list would be operationally similar to accessing a DB via entity framework
        public static void InitilizeCharacterArray(List<Character> characters)
        {
            _characters = characters;
        }

        //defining these methods as async because typically this would be IO, even though it's not currently
        public async Task<Character> LoadByCharacterName(string characterName)
        {
            return _characters.Find(x => x.Name == characterName);
        }

        public async Task SaveCharacter(Character character)
        {
            int characterIndex = _characters.FindIndex(x => x.Name == character.Name);
            if (characterIndex == -1) throw new Exception("Character not found");
            _characters[characterIndex] = character;
        }
    }
}
