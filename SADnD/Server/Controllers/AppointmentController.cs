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
    public class AppointmentController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<Appointment, ApplicationDbContext> _appointmentManager;
        EFRepositoryGeneric<Campaign, ApplicationDbContext> _campaignManager;
        public AppointmentController(
            UserManager<ApplicationUser> userManager, 
            EFRepositoryGeneric<Appointment, ApplicationDbContext> appointmentManager,
            EFRepositoryGeneric<Campaign, ApplicationDbContext> campaignManager)
        {
            _userManager = userManager;
            _appointmentManager = appointmentManager;
            _campaignManager = campaignManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Appointment>>> GetAllAppointments()
        {
            try
            {
                var campaignIds = User.Claims.Where(c => c.Type == "Campaign").Select(c => c.Value);
                var result = await _appointmentManager.Get(a => campaignIds.Contains(a.CampaignId),null,"AppointmentVotes");
                return Ok(new APIListOfEntityResponse<Appointment>()
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
        public async Task<ActionResult<APIEntityResponse<Appointment>>> GetByAppointmentId(int id)
        {
            try
            {
                var result = (await _appointmentManager.Get(x => x.Id == id,null,"AppointmentVotes")).FirstOrDefault();
                var campaignIds = User.Claims.Where(c => c.Type == "Campaign").Select(c => c.Value);
                if (!campaignIds.Contains(result.CampaignId))
                    return StatusCode(403);
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Appointment Not Found" },
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
        public async Task<ActionResult<APIEntityResponse<Appointment>>> Post([FromBody] Appointment appointment)
        {
            try
            {
                var campaignRoles = User.Claims.Where(c => c.Type == "CampaignRole").Select(c => c.Value);
                if (!campaignRoles.Contains($"{appointment.CampaignId}:DungeonMaster"))
                    return StatusCode(403);
                await _appointmentManager.Insert(appointment);
                var result = (await _appointmentManager.Get(x => x.Id == appointment.Id,null,"AppointmentVotes")).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find appointment after adding it" },
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
        public async Task<ActionResult<APIEntityResponse<Appointment>>> Put([FromBody] Appointment appointment)
        {
            try
            {
                var campaignIds = User.Claims.Where(c => c.Type == "Campaign").Select(c => c.Value);
                var campaignRoles = User.Claims.Where(c => c.Type == "CampaignRole").Select(c => c.Value);
                if (!campaignIds.Contains(appointment.CampaignId) || (appointment.Accepted && !campaignRoles.Contains($"{appointment.CampaignId}:DungeonMaster")))
                    return StatusCode(403);
                await _appointmentManager.Update(appointment);
                var result = (await _appointmentManager.Get(x => x.Id == appointment.Id,null,"AppointmentVotes")).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Appointment>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find appointment after updating it" },
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
                var appointment = await _appointmentManager.GetByID(id);
                if (appointment != null)
                {
                    var campaignRoles = User.Claims.Where(c => c.Type == "CampaignRole").Select(c => c.Value);
                    if (!campaignRoles.Contains($"{appointment.CampaignId}:DungeonMaster"))
                        return StatusCode(403);
                    var success = await _appointmentManager.Delete(id);
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
