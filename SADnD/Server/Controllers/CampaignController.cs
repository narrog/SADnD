using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SADnD.Server.Data;
using SADnD.Shared.Models;

namespace SADnD.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<Campaign,ApplicationDbContext> _campaignManager;
        public CampaignController(UserManager<ApplicationUser> userManager, EFRepositoryGeneric<Campaign,ApplicationDbContext> campaignManager)
        {
            _userManager = userManager;
            _campaignManager = campaignManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Campaign>>> GetAllCampaigns()
        {
            return StatusCode(403);

            //try
            //{
            //    var result = await _campaignManager.GetAll();
            //    return Ok(new APIListOfEntityResponse<Campaign>()
            //    {
            //        Success = true,
            //        Data = result
            //    });
            //}
            //catch (Exception ex)
            //{
            //    // TODO: log Exception
            //    return StatusCode(500);
            //}
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

        [HttpPost]
        public async Task<ActionResult<APIEntityResponse<Campaign>>> Post([FromBody] Campaign campaign)
        {
            try
            {
                while ((await _campaignManager.GetByID(campaign.Id)) != null)
                {
                    campaign.RegenerateId();
                }
                await _campaignManager.Insert(campaign);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (await _campaignManager.Get(x => x.Id == id) != null)
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
