using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SADnD.Server.Areas.Identity;
using SADnD.Server.Data;
using SADnD.Shared.Models;
using System.Security.Claims;

namespace SADnD.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        CharacterManager _characterManager;
        public CharacterController(
            UserManager<ApplicationUser> userManager, 
            CharacterManager campaignManager)
        {
            _userManager = userManager;
            _characterManager = campaignManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Character>>> GetAllCharacters()
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _characterManager.Get(x => x.UserId == id,null,"Race,Classes.Class");
                return Ok(new APIListOfEntityResponse<Character>()
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIEntityResponse<Character>>> GetByCharacterId(int id)
        {
            try
            {
                var result = (await _characterManager.GetByID(id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Character Not Found" },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIEntityResponse<Character>>> Post([FromBody] Character character)
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    character.User = user;
                }
                await _characterManager.Insert(character);
                var result = (await _characterManager.GetByID(character.Id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find character after adding it" },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIEntityResponse<Character>>> Put([FromBody] Character character)
        {
            try
            {
                await _characterManager.Update(character);
                var result = (await _characterManager.Get(x => x.Id == character.Id)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Character>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find character after updating it" },
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _characterManager.Get(x => x.Id == id) != null) 
                {
                    var success = await _characterManager.Delete(id);
                    if (success)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }
    }
}
