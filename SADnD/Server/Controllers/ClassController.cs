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
    public class ClassController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<Class, ApplicationDbContext> _classManager;
        public ClassController(
            UserManager<ApplicationUser> userManager, 
            EFRepositoryGeneric<Class, ApplicationDbContext> classManager)
        {
            _userManager = userManager;
            _classManager = classManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<Class>>> GetAllClasses()
        {
            try
            {
                var result = await _classManager.GetAll();
                return Ok(new APIListOfEntityResponse<Class>()
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
        public async Task<ActionResult<APIEntityResponse<Class>>> GetByClassId(int id)
        {
            try
            {
                var result = (await _classManager.GetByID(id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<Class>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<Class>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Class Not Found" },
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
        //public async Task<ActionResult<APIEntityResponse<Class>>> Post([FromBody] Class class)
        //{
        //    try
        //    {
        //        await _classManager.Insert(class);
        //        var result = (await _classManager.Get(x => x.Id == class.Id)).FirstOrDefault();
        //        if (result != null)
        //        {
        //            return Ok(new APIEntityResponse<Class>()
        //            {
        //                Success = true,
        //                Data = result
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new APIEntityResponse<Class>()
        //            {
        //                Success = false,
        //                ErrorMessages = new List<string>() { "Could not find class after adding it" },
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
        //public async Task<ActionResult<APIEntityResponse<Class>>> Put([FromBody] Class class)
        //{
        //    try
        //    {
        //        await _classManager.Update(class);
        //        var result = (await _classManager.Get(x => x.Id == class.Id)).FirstOrDefault();
        //        if (result != null)
        //        {
        //            return Ok(new APIEntityResponse<Class>()
        //            {
        //                Success = true,
        //                Data = result
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new APIEntityResponse<Class>()
        //            {
        //                Success = false,
        //                ErrorMessages = new List<string>() { "Could not find class after updating it" },
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
        //        if (await _classManager.Get(x => x.Id == id) != null)
        //        {
        //            var success = await _classManager.Delete(id);
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
