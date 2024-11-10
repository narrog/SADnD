using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SADnD.Client.Pages.CampaignPages;
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
    public class JoinRequestController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<JoinRequest,ApplicationDbContext> _requestManager;
        EFRepositoryGeneric<Campaign, ApplicationDbContext> _campaignManager;
        public JoinRequestController(
            UserManager<ApplicationUser> userManager,
            EFRepositoryGeneric<JoinRequest, ApplicationDbContext> requestManager,
            EFRepositoryGeneric<Campaign, ApplicationDbContext> campaignManager)
        {
            _userManager = userManager;
            _requestManager = requestManager;
            _campaignManager = campaignManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<JoinRequest>>> GetAllRequests()
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                var result = await _requestManager.Get(x => x.Campaign.DungeonMasters.Any(dm => dm.Id == id) || x.UserId == id,null,"Campaign,User");
                return Ok(new APIListOfEntityResponse<JoinRequest>()
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
        public async Task<ActionResult<APIEntityResponse<JoinRequest>>> GetByRequestId(int id)
        {
            try
            {
                var result = (await _requestManager.Get(x => x.Id == id,null,"Campaign,User")).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Request Not Found" },
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
        public async Task<ActionResult<APIEntityResponse<JoinRequest>>> Post([FromBody] JoinRequest request)
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (request.UserId != id)
                {
                    return StatusCode(403);
                }

                var campaign = (await _campaignManager.Get(c => c.Id == request.CampaignId,null,"DungeonMasters,Players")).FirstOrDefault();
                if (campaign == null)
                {
                    return StatusCode(404);
                }
                if (campaign.Players.Any(p => p.Id == id) || campaign.DungeonMasters.Any(dm =>  dm.Id == id))
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = false,
                        ErrorMessages = new List<string> { "Sie sind bereits Mitglied dieser Kampagne" },
                        Data = null
                    });
                }

                await _requestManager.Insert(request);
                var result = await _requestManager.GetByID(request.Id);
                if (result != null)
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find request after adding it" },
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
        public async Task<ActionResult<APIEntityResponse<JoinRequest>>> Put([FromBody] JoinRequest request)
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var campaign = (await _campaignManager.Get(c => c.Id == request.CampaignId, null, "DungeonMasters,Players")).FirstOrDefault();
                if (!campaign.DungeonMasters.Any(dm => dm.Id == id) && request.Accepted != null)
                {
                    return StatusCode(404);
                }
                if (campaign.Players.Any(p => p.Id == id) || !campaign.DungeonMasters.Any(dm => dm.Id == id)) 
                {
                    return StatusCode(403);
                }
                await _requestManager.Update(request);
                if (request.Accepted != null)
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);
                    campaign.Players.Add(user);
                    await _campaignManager.Update(campaign);
                }
                var result = (await _requestManager.GetByID(request.Id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<JoinRequest>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find request after updating it" },
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
                var userId = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var request = (await _requestManager.Get(r => r.Id == id,null,"User,Campaign.DungeonMasters")).FirstOrDefault();
                if (request != null && (request.User.Id == userId || request.Campaign.DungeonMasters.Any(dm => dm.Id == userId))) 
                {
                    var success = await _requestManager.Delete(id);
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
