using CharacterManagement.API;
using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.RequestObjects;
using CharacterManagement.Domain;
using CharacterManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace CharacterManagement.Test
{
    public class CharacterHealthServiceTest
    {
        private CharacterHealthService InitilizeEnvironment()
        {
            //Typically would use a faking library to fake the repository and not use concrete classes, but since the repository is fake anyway this seemed easier for now and gets the general point across
            CharacterRepository characterRepository = new CharacterRepository();
            CharacterHealthService characterHealthService = new CharacterHealthService(characterRepository);
            List<Character> characters = new List<Character>();
            characters.Add(DemoStartup.InitilizeDemoCharacter(characterHealthService));
            CharacterRepository.InitilizeCharacterArray(characters);
            return characterHealthService;
        }
        [Fact]
        public async void HealCharacter_PassingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Character character = await characterHealthService.HealCharacter("Briv", 5);
            Assert.True(character.HP.Current == character.HP.Max); //no damage done to character yet, should be max
            CharacterDamage[] characterDamages = new CharacterDamage[1];
            characterDamages[0] = new CharacterDamage()
            {
                Amount = 10,
                Type = "piercing"
            };
            await characterHealthService.DamageCharacter("Briv", characterDamages);
            character = await characterHealthService.HealCharacter("Briv", 5);
            Assert.True(character.HP.Current == character.HP.Max - 5);
        }

        [Fact]
        public async void HealCharacter_InvalidArguments_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => characterHealthService.HealCharacter("", 5));
            Assert.Equal("Invalid arguments", ex.Message);
        }

        [Fact]
        public async void HealCharacter_InvalidCharacter_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => characterHealthService.HealCharacter("TEST", 5));
            Assert.Equal("Character not found for identifier", ex.Message);
        }

        [Fact]
        public async void AddTempHP_PassingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Character character = await characterHealthService.AddTempHPToCharacter("Briv", 5);
            Assert.True(character.HP.Total == character.HP.Max + 5); //no damage done to character yet, should be max            
        }

        [Fact]
        public async void AddTempHP_InvalidArguments_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => characterHealthService.AddTempHPToCharacter("", 5));
            Assert.Equal("Invalid arguments", ex.Message);
        }

        [Fact]
        public async void AddTempHP_InvalidCharacter_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => characterHealthService.AddTempHPToCharacter("TEST", 5));
            Assert.Equal("Character not found for identifier", ex.Message);
        }

        [Fact]
        public async void DamageCharacter_PassingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Character character = await characterHealthService.AddTempHPToCharacter("Briv", 5);            
            CharacterDamage[] characterDamages = new CharacterDamage[1];
            characterDamages[0] = new CharacterDamage()
            {
                Amount = 4,
                Type = "piercing"
            };
            //testing that the health correctly removes from temp before it removes from current health
            character = await characterHealthService.DamageCharacter("Briv", characterDamages);
            Assert.True(character.HP.Total == character.HP.Max + 1);
            character = await characterHealthService.DamageCharacter("Briv", characterDamages);
            Assert.True(character.HP.Total == character.HP.Max - 3);
            await characterHealthService.HealCharacter("Briv", 3); //bring back to full for next part
            characterDamages = new CharacterDamage[4];
            //testing damage that includes a resistance, immunity, and normal. we've tested normal already above. This also tests that a null record should be skipped correct, the 4th index is not initilized
            characterDamages[0] = new CharacterDamage()
            {
                Amount = 10,
                Type = "fire"
            };
            characterDamages[1] = new CharacterDamage()
            {
                Amount = 5,
                Type = "slashing"
            };
            characterDamages[2] = new CharacterDamage()
            {
                Amount = 2,
                Type = "psychic"
            };
            character = await characterHealthService.DamageCharacter("Briv", characterDamages); //This should deal 4 damage total because the fire is restited
            Assert.True(character.HP.Total == character.HP.Max - 4);
        }

        [Fact]
        public async void DamageCharacter_InvalidArguments_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<ArgumentException>(() => characterHealthService.DamageCharacter("", new CharacterDamage[1]));
            Assert.Equal("Invalid arguments", ex.Message);
        }

        [Fact]
        public async void DamageCharacter_InvalidCharacter_FailingTest()
        {
            CharacterHealthService characterHealthService = InitilizeEnvironment();
            Exception ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => characterHealthService.DamageCharacter("TEST", new CharacterDamage[1]));
            Assert.Equal("Character not found for identifier", ex.Message);
        }
    }
}
