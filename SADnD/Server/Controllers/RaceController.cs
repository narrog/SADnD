using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SADnD.Server.Areas.Identity;
using SADnD.Server.Data;
using SADnD.Shared;
using SADnD.Shared.Models;
using System.Security.Claims;

namespace SADnD.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<Race, ApplicationDbContext> _raceManager;
        public RaceController(
            UserManager<ApplicationUser> userManager, 
            EFRepositoryGeneric<Race, ApplicationDbContext> raceManager)
        {
            _userManager = userManager;
            _raceManager = raceManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Race>>> GetAllRaces()
        {
            try
            {
                var result = await _raceManager.GetAll();
                return Ok(new APIListOfEntityResponse<Race>()
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
        public async Task<ActionResult<APIEntityResponse<Race>>> GetByRaceId(int id)
        {
            try
            {
                var result = (await _raceManager.GetByID(id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Race>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Race>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Race Not Found" },
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

        //[HttpPost]
        //public async Task<ActionResult<APIEntityResponse<Race>>> Post([FromBody] Race race)
        //{
        //    try
        //    {
        //        await _raceManager.Insert(race);
        //        var result = (await _raceManager.Get(x => x.Id == race.Id)).FirstOrDefault();
        //        if (result != null)
        //        {
        //            return Ok(new APIEntityResponse<Race>()
        //            {
        //                Success = true,
        //                Data = result
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new APIEntityResponse<Race>()
        //            {
        //                Success = false,
        //                ErrorMessages = new List<string>() { "Could not find race after adding it" },
        //                Data = null
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: log Exception
        //        return StatusCode(500);
        //    }
        //}

        //[HttpPut]
        //public async Task<ActionResult<APIEntityResponse<Race>>> Put([FromBody] Race race)
        //{
        //    try
        //    {
        //        await _raceManager.Update(race);
        //        var result = (await _raceManager.Get(x => x.Id == race.Id)).FirstOrDefault();
        //        if (result != null)
        //        {
        //            return Ok(new APIEntityResponse<Race>()
        //            {
        //                Success = true,
        //                Data = result
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new APIEntityResponse<Race>()
        //            {
        //                Success = false,
        //                ErrorMessages = new List<string>() { "Could not find race after updating it" },
        //                Data = null
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: log Exception
        //        return StatusCode(500);
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        if (await _raceManager.Get(x => x.Id == id) != null)
        //        {
        //            var success = await _raceManager.Delete(id);
        //            if (success)
        //            {
        //                return NoContent();
        //            }
        //            else
        //            {
        //                return StatusCode(500);
        //            }
        //        }
        //        else
        //        {
        //            return StatusCode(500);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: log Exception
        //        return StatusCode(500);
        //    }
        //}
    }
}
