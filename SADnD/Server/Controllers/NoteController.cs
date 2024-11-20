using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
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
    public class NoteController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<Note, ApplicationDbContext> _noteManager;
        public NoteController(
            UserManager<ApplicationUser> userManager, 
            EFRepositoryGeneric<Note, ApplicationDbContext> noteManager)
        {
            _userManager = userManager;
            _noteManager = noteManager;
        }
        private Dictionary<string, Type> typeMapping = new Dictionary<string, Type>()
        {
            {"NoteStory",typeof(NoteStory)},
            {"NotePerson",typeof(NotePerson)},
            {"NoteLocation",typeof(NoteLocation)},
            {"NoteQuest",typeof(NoteQuest)},
            {"NoteHint",typeof(NoteHint)}
        };

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Note>>> GetAllNotes()
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _noteManager.Get(n => n.UserId == id);
                return Ok(new APIListOfEntityResponse<Note>()
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
        public async Task<ActionResult<APIEntityResponse<Note>>> GetByNoteId(int id)
        {
            try
            {
                var userId = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _noteManager.GetByID(id);
                if (result != null && result.UserId == userId)
                {
                    return Ok(new APIEntityResponse<Note>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Note>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Note Not Found" },
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
        public async Task<ActionResult<APIEntityResponse<Note>>> Post([FromBody] JObject jsNote)
        {
            try
            {
                var note = (Note)jsNote.ToObject(typeMapping[jsNote["type"]?.ToString()]);

                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (note.UserId == null || note.UserId != id)
                {
                    return StatusCode(403);
                }
                if (note.CampaignId != null ^ note.CharacterId != null)
                {
                    var type = note.GetType();
                    await _noteManager.Insert(note);
                    var result = (await _noteManager.Get(x => x.Id == note.Id)).FirstOrDefault();
                    if (result != null)
                    {
                        return Ok(new APIEntityResponse<Note>()
                        {
                            Success = true,
                            Data = result
                        });
                    }
                    else
                    {
                        return Ok(new APIEntityResponse<Note>()
                        {
                            Success = false,
                            ErrorMessages = new List<string>() { "Could not find note after adding it" },
                            Data = null
                        });
                    }
                }
                return StatusCode(400, "Note has to be bound to Campaign or Character, not to both");
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<APIEntityResponse<Note>>> Put([FromBody] JObject jsNote)
        {
            try
            {
                var note = (Note)jsNote.ToObject(typeMapping[jsNote["type"]?.ToString()]);

                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (note.UserId == null || note.UserId != id)
                {
                    return StatusCode(403);
                }
                await _noteManager.Update(note);
                var result = (await _noteManager.Get(x => x.Id == note.Id)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Note>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Note>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find note after updating it" },
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
                var target = (await _noteManager.Get(x => x.Id == id)).FirstOrDefault();
                if (target != null)
                {
                    var userId = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                    if (target.UserId == userId)
                    {
                        var success = await _noteManager.Delete(id);
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
                        return StatusCode(403);
                }
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                // TODO: log Exception
                return StatusCode(500);
            }
        }
    }
}
