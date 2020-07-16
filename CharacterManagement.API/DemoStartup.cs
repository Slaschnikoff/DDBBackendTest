using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharacterManagement.API
{
    /// <summary>
    /// Created this to handle mocking up objects for the demo so it's all the hard coded stuff is in one place to replace later if needed
    /// </summary>
    public class DemoStartup
    {

        public static Character InitilizeDemoCharacter(ICharacterHealthService characterHealthService)
        {
            Character character = GetDemoCharacter();
            character.HP.Max = characterHealthService.CalculateMaxHP(character);
            character.HP.Current = character.HP.Max;
            return character;
        }

        private static Character GetDemoCharacter()
        {
            var testCharacter = new Character("Briv")
            {
                Stats = new Stat()
                {
                    Strength = new Contracts.Entities.Attribute(15),
                    Dexterity = new Contracts.Entities.Attribute(12),
                    Constitution = new Contracts.Entities.Attribute(14),
                    Intelligence = new Contracts.Entities.Attribute(13),
                    Wisdom = new Contracts.Entities.Attribute(10),
                    Charisma = new Contracts.Entities.Attribute(8)
                }
                ,
                Level = 5
            };
            testCharacter.Classes.Add(new Class()
            {
                Name = "fighter",
                HitDiceValue = 10,
                ClassLevel = 3
            });
            testCharacter.Classes.Add(new Class()
            {
                Name = "wizard",
                HitDiceValue = 6,
                ClassLevel = 2
            });
            testCharacter.Items.Add(new Item()
            {
                Name = "Ioun Stone of Fortitude",
                Modifier = new Modifier()
                {
                    AffectedObject = "stats",
                    AffectedValue = "constitution",
                    Value = 2
                }
            });
            testCharacter.Defenses.Add(new DefenseValue()
            {
                Type = "fire",
                Defense = "immunity"
            });
            testCharacter.Defenses.Add(new DefenseValue()
            {
                Type = "slashing",
                Defense = "resistance"
            });
            return testCharacter;
        }

    }
}
