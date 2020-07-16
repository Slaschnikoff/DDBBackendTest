using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.RequestObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Contracts.Services
{
    public interface ICharacterHealthService
    {
        /// <summary>
        /// Loads the character from the infrastructure and applies any healing, and then saves it
        /// </summary>
        /// <param name="characterName">The character name to load</param>
        /// <param name="amount"></param>
        /// <returns>The updated character after any alterations</returns>
        Task<Character> HealCharacter(string characterName, int amount);
        /// <summary>
        /// Applies damage to a character, adhering to any resistances/immunities applicable. This method loads the character, alters it, then saves it
        /// </summary>
        /// <param name="characterName">The character name to load</param>
        /// <param name="damage"></param>
        /// <returns>The updated character after any alterations</returns>
        Task<Character> DamageCharacter(string characterName, CharacterDamage[] damages);
        /// <summary>
        /// Adds temp HP to a character. This method loads the character, alters it, then saves it
        /// </summary>
        /// <param name="characterNamer">The character name to load</param>
        /// <param name="amount"></param>
        /// <returns>The updated character after any alterations</returns>
        Task<Character> AddTempHPToCharacter(string characterName, int amount);
        /// <summary>
        /// Calcualtes a character's MAx HP based on their items and stats.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        int CalculateMaxHP(Character character);
    }
}
