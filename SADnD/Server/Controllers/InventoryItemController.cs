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
    public class InventoryItemController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        EFRepositoryGeneric<InventoryItem, ApplicationDbContext> _itemManager;
        public InventoryItemController(
            UserManager<ApplicationUser> userManager, 
            EFRepositoryGeneric<InventoryItem, ApplicationDbContext> itemManager)
        {
            _userManager = userManager;
            _itemManager = itemManager;
        }

        [HttpGet]
        public async Task<ActionResult<APIListOfEntityResponse<InventoryItem>>> GetAllItems()
        {
            try
            {
                var id = User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _itemManager.Get(i => i.UserId == id);
                return Ok(new APIListOfEntityResponse<InventoryItem>()
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
        public async Task<ActionResult<APIEntityResponse<InventoryItem>>> GetByInventoryItemId(int id)
        {
            try
            {
                var result = (await _itemManager.GetByID(id));
                if (result != null)
                {
                    return Ok(new APIEntityResponse<InventoryItem>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<InventoryItem>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "InventoryItem Not Found" },
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
        public async Task<ActionResult<APIEntityResponse<InventoryItem>>> Post([FromBody] InventoryItem item)
        {
            try
            {
                await _itemManager.Insert(item);
                var result = (await _itemManager.Get(x => x.Id == item.Id)).FirstOrDefault();
                if (result != null)
                {
                    return Ok(new APIEntityResponse<InventoryItem>()
                    {
                        Success = true,
                        Data = result
                    });
                }
                else
                {
                    return Ok(new APIEntityResponse<InventoryItem>()
                    {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find item after adding it" },
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
        public async Task<ActionResult<APIEntityResponse<InventoryItem>>> Put([FromBody] InventoryItem item) {
            try {
                await _itemManager.Update(item);
                var result = (await _itemManager.Get(x => x.Id == item.Id)).FirstOrDefault();
                if (result != null) {
                    return Ok(new APIEntityResponse<InventoryItem>() {
                        Success = true,
                        Data = result
                    });
                }
                else {
                    return Ok(new APIEntityResponse<InventoryItem>() {
                        Success = false,
                        ErrorMessages = new List<string>() { "Could not find item after updating it" },
                        Data = null
                    });
                }
            }
            catch (Exception ex) {
                // TODO: log Exception
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            try {
                if (await _itemManager.Get(x => x.Id == id) != null) {
                    var success = await _itemManager.Delete(id);
                    if (success) {
                        return NoContent();
                    }
                    else {
                        return StatusCode(500);
                    }
                }
                else {
                    return StatusCode(500);
                }
            }
            catch (Exception ex) {
                // TODO: log Exception
                return StatusCode(500);
            }
        }
    }
}
