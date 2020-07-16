using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterManagement.Contracts.Entities;
using CharacterManagement.Contracts.RequestObjects;
using CharacterManagement.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CharacterManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]/{characterName}")]
    public class CharacterController : ControllerBase
    {        
        private readonly ILogger<CharacterController> _logger;
        private readonly ICharacterHealthService _characterHealthService;
        private readonly ICharacterLoadService _characterLoadService;

        public CharacterController(ILogger<CharacterController> logger, ICharacterHealthService characterHealthService, ICharacterLoadService characterLoadService)
        {
            _logger = logger;
            _characterHealthService = characterHealthService;
            _characterLoadService = characterLoadService;
        }

        [HttpGet("hp")]
        [ProducesResponseType(typeof(CharacterHealth), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCurrentHealth(string characterName)
        {
            try
            {
                return StatusCode(200, (await _characterLoadService.LoadCharacterByName(characterName)).HP);
            }
            catch (KeyNotFoundException keyEx) { return StatusCode(404); }
            catch (ArgumentException argEx) { return StatusCode(400); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get HP");
                return StatusCode(500);
            }            
        }

        [HttpPost("hp/Heal")]
        [ProducesResponseType(typeof(CharacterHealth), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> HealCharacter(string characterName, HealthChange healthChange)
        {
            try
            {
                return StatusCode(200, (await _characterHealthService.HealCharacter(characterName, healthChange.Amount)).HP);
            }
            catch (KeyNotFoundException keyEx) { return StatusCode(404); }
            catch (ArgumentException argEx) { return StatusCode(400); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in heal character");
                return StatusCode(500);
            }
        }

        [HttpPost("hp/AddTemp")]
        [ProducesResponseType(typeof(CharacterHealth), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddTempHP(string characterName, HealthChange healthChange)
        {
            try
            {
                return StatusCode(200, (await _characterHealthService.AddTempHPToCharacter(characterName, healthChange.Amount)).HP);
            }
            catch (KeyNotFoundException keyEx) { return StatusCode(404); }
            catch (ArgumentException argEx) { return StatusCode(400); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in add temp hp to character");
                return StatusCode(500);
            }
        }

        [HttpPost("hp/Damage")]
        [ProducesResponseType(typeof(CharacterHealth), 200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DamageCharacter(string characterName, DamageDTO damageDTO)
        {
            try
            {
                return StatusCode(200, (await _characterHealthService.DamageCharacter(characterName, damageDTO.Damage)).HP);
            }
            catch (KeyNotFoundException keyEx) { return StatusCode(404); }
            catch (ArgumentException argEx) { return StatusCode(400); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in damage character");
                return StatusCode(500);
            }
        }

    }
}
