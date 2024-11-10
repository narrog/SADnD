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
    public class CampaignController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        CustomClaimsService<ApplicationDbContext,UserManager<ApplicationUser>> _customClaimsService;
        EFRepositoryGeneric<Campaign,ApplicationDbContext> _campaignManager;
        public CampaignController(
            UserManager<ApplicationUser> userManager, 
            CustomClaimsService<ApplicationDbContext, UserManager<ApplicationUser>> customClaimsService, 
            EFRepositoryGeneric<Campaign,ApplicationDbContext> campaignManager)
        {
            _userManager = userManager;
            _customClaimsService = customClaimsService;
            _campaignManager = campaignManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Campaign>>> GetAllCampaigns()
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                var result = await _campaignManager.Get(x => x.DungeonMasters.Any(dm  => dm.Id == user.Id) || x.Players.Any(p => p.Id == user.Id));
                return Ok(new APIListOfEntityResponse<Campaign>()
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
        public async Task<ActionResult<APIEntityResponse<Campaign>>> GetByCampaignId(string id)
        {
            try
            {
                var result = (await _campaignManager.Get(x => x.Id == id.ToUpper())).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Campaign Not Found" },
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
        //[HttpGet("{DungeonMaster}/searchbydungeonmaster")]
        //public async Task<ActionResult<APIEntityResponse<Campaign>>> SearchByDungeonMaster(string id) {
        //    try {
        //        var result = (await _campaignManager.Get(x => x.Id == id.ToUpper())).FirstOrDefault();
        //        if (result != null) {
        //            return Ok(new APIEntityResponse<Campaign>() {
        //                Success = true,
        //                Data = result
        //            });
        //        }
        //        else {
        //            return Ok(new APIEntityResponse<Campaign>() {
        //                Success = false,
        //                ErrorMessages = new List<string>() { "Campaign Not Found" },
        //                Data = null
        //            });
        //        }
        //    }
        //    catch (Exception ex) {
        //        // TODO: log Exception
        //        return StatusCode(500);
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<APIEntityResponse<Campaign>>> Post([FromBody] Campaign campaign)
        {
            try
            {
                while ((await _campaignManager.GetByID(campaign.Id)) != null)
                {
                    campaign.RegenerateId();
                }
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    campaign.DungeonMasters = new List<ApplicationUser>() { user};
                }
                await _campaignManager.Insert(campaign);
                var result = (await _campaignManager.Get(x => x.Id == campaign.Id)).FirstOrDefault();
                if (result != null)
                {

                    await _customClaimsService.AddCampaignClaims(user);
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find campaign after adding it" },
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
        public async Task<ActionResult<APIEntityResponse<Campaign>>> Put([FromBody] Campaign campaign)
        {
            try
            {
                await _campaignManager.Update(campaign);
                var result = (await _campaignManager.Get(x => x.Id == campaign.Id)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Campaign>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find campaign after updating it" },
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

        [Authorize(Policy="IsDungeonMaster")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (await _campaignManager.Get(x => x.Id == id) != null) //&& User.HasClaim("CampaignRole", $"{id}:DungeonMaster"))
                {
                    var success = await _campaignManager.Delete(id);
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
