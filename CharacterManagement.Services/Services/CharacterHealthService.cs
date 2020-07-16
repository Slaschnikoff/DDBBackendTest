using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.Repositories;
using CharacterManagement.Contracts.RequestObjects;
using CharacterManagement.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterManagement.Domain
{
    public class CharacterHealthService : ICharacterHealthService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterHealthService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }        

        public async Task<Character> AddTempHPToCharacter(string characterName, int amount)
        {
            if (characterName.Length == 0) throw new ArgumentException("Invalid arguments");

            Character character = await _characterRepository.LoadByCharacterName(characterName);

            if (character == null) throw new KeyNotFoundException("Character not found for identifier");

            if (character.HP.Temp > amount) return character; //The current HP is higher than the supplied amount, do nothing.

            character.HP.Temp = amount;

            await _characterRepository.SaveCharacter(character);

            return character;
        }

        public async Task<Character> DamageCharacter(string characterName, CharacterDamage[] damages)
        {
            if (characterName.Length == 0) throw new ArgumentException("Invalid arguments");

            Character character = await _characterRepository.LoadByCharacterName(characterName);

            if (character == null) throw new KeyNotFoundException("Character not found for identifier");

            int totalDamageDealt = CalculateTotalDamage(character, damages);
            ApplyDamage(character, totalDamageDealt);            

            await _characterRepository.SaveCharacter(character);

            return character;
        }

        private void ApplyDamage(Character character, int totalDamageDealt)
        {
            //Apply damage to temp HP first
            if (character.HP.Temp > 0)
            {
                if (character.HP.Temp < totalDamageDealt)
                {
                    totalDamageDealt -= character.HP.Temp;
                    character.HP.Temp = 0;
                }
                else
                {
                    character.HP.Temp -= totalDamageDealt;
                    return; //Total damage dealt wasn't enough to get temp HP past 0, so we can return, nothing to apply to the current health
                }
            }

            character.HP.Current -= totalDamageDealt;          
            //Could see if HP is below 0 and set to 0, however another method may want to check how negative HP went to determine if Massive Damage is initiated 
        }

        private int CalculateTotalDamage(Character character, CharacterDamage[] damages)
        {
            int totalDamageDealt = 0;
            for (int i = 0; i < damages.Length; i++)
            {
                //Grabbing a collection of the defenses assuming this isn't a calculated end result of defenses, and is instead a columniation of all
                //  resistences applied by features, in which case you could have resitance and immunity to the same type of damage;
                DefenseValue[] defenseValues = character.Defenses.Where(x => x.Type == damages[i].Type).ToArray();
                if (defenseValues.Length == 0)
                {
                    totalDamageDealt += damages[i].Amount;
                    continue; //No resistances found, move on
                }
                //If the defense contains an immunity, then we don't need to increase total damage dealt. If immunity doesn't exiost, then it's resistance and we can add half
                if (!Array.Exists(defenseValues, x => x.Defense == "immunity"))
                    totalDamageDealt += damages[i].Amount / 2;
            }
            return totalDamageDealt;
        }

        public async Task<Character> HealCharacter(string characterName, int amount)
        {
            if (characterName.Length == 0) throw new ArgumentException("Invalid arguments");

            Character character = await _characterRepository.LoadByCharacterName(characterName);

            if (character == null) throw new KeyNotFoundException("Character not found for identifier");

            character.HP.Current += amount;

            if (character.HP.Current > character.HP.Max) character.HP.Current = character.HP.Max; //Can't go above max health

            await _characterRepository.SaveCharacter(character);

            return character;
        }

        public int CalculateMaxHP(Character character)
        {
            if (character == null) throw new ArgumentException("Invalid arguments");
            int MaxHP = 0;            
            for (int i = 0; i < character.Classes.Count; i++)
            {
                MaxHP += CalculateAverageClassHitDie(character.Classes[i], i);
            }
            MaxHP += (CalculateCharacterItemConBonus(character) + character.Stats.Constitution.Modifier) * character.Level;
            return MaxHP;
        }

        private int CalculateAverageClassHitDie(Class characterClass, int classIndex)
        {
            int classHP = 0;
            int classMultiplier = characterClass.ClassLevel;
            //For now assuming that the character is using the average logic for HP calc for simplicity. Assuming currently in the data structure that the            
            //first class in the classes array was the class taken at first level, so for that level you get max HP.
            if (classIndex == 0)
            {
                classMultiplier -= 1;
                classHP += characterClass.HitDiceValue;
            }
            
            classHP += ((characterClass.HitDiceValue / 2) + 1) * classMultiplier; //the average of a die is half + 1

            return classHP;
        }

        private int CalculateCharacterItemConBonus(Character character)
        {
            //find any items that affect con
            Item[] conItems = character.Items.Where(x => x.Modifier.AffectedObject == "stats" && x.Modifier.AffectedValue == "constitution").ToArray();
            int itemBonusCon = 0;
            for (int i = 0; i < conItems.Count(); i++)
            {
                itemBonusCon += conItems[i].Modifier.Value / 2; //we only want the modifier, the score is irrelevent for HP calculation
            }
            return itemBonusCon;
        }
    }
}
