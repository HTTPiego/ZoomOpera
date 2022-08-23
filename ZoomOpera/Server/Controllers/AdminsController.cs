using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {

        private readonly IService<IAdmin, AdminDTO> _adminService;

        public AdminsController(IService<IAdmin, AdminDTO> adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<IAdmin>>> GetAdmins()
        {
            try
            {
                return Ok(await _adminService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpGet("{idAdmin}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IAdmin>> GetAdmin(Guid idAdmin)
        {
            try
            {
                var result = await _adminService.GetEntity(idAdmin);

                if (result == null) 
                    return NotFound($"Employee with Id = {idAdmin} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpDelete("{idAdmin}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IAdmin>> DeleteAdmin(Guid idAdmin)
        {
            try
            {
                //TODO: non bellissimo perche il find viene rifatto dopo pero dovrebbe essere cachata e la richiesta al database NON rifatta: 
                //bene ma non benissimo
                var adminToDelete = await _adminService.GetEntity(idAdmin);

                if (adminToDelete == null)
                {
                    return NotFound($"Employee with Id = {idAdmin} not found");
                }

                return Ok(await _adminService.DeleteEntity(idAdmin));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }


        [HttpPost]
        //TODO: _____IMPORTANTE____ aggiornare l'authorize 
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IAdmin>> CreateAdmin([FromBody]AdminDTO admin)
        {
            try
            {
                if (AdminIsNotValid(admin))
                    return BadRequest("Admin is not valid");

                if (this.NameAlreadyTaken(admin))
                    return BadRequest($"Title \"{admin.Name}\" has been already taken");

                if (this.TwoEqualAdmins(admin))
                    return BadRequest("This admin has been already registered");

                return Ok(await _adminService.AddEntity(admin));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new admin record");
            }
        }

        private bool AdminIsNotValid(AdminDTO dto)
        {
            if (dto == null)
                return true;
            if (dto.Name == null
                || dto.Password == null
                || dto.Email == null
                || dto.GivenName == null
                || dto.Surname == null)
                return true;
            return false;
        }

        private bool NameAlreadyTaken(AdminDTO dto)
        {
            return this._adminService.FindFirstBy(dbAdmin =>
                        new ValueTask<bool>(dbAdmin.Name.ToLower().Equals(dto.Name.ToLower()))).Result != null;
        }

        private bool TwoEqualAdmins(AdminDTO dto)
        {
            return this._adminService.FindFirstBy(dbAdmin => new ValueTask<bool>(dbAdmin.Equals(dto))).Result != null;
        }


        [HttpPut("{adminId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IAdmin>> UpdateAdmin(Guid adminId, [FromBody] AdminDTO dto)
        {
            try
            {
                if (AdminIsNotValid(dto))
                    return BadRequest("Admin is not valid");

                if (this.AdminCanNotBeUpdated(dto, adminId))
                    return BadRequest("A similar admin has been already registered");

                var adminToUpdate = await _adminService.GetEntity(adminId);

                if (adminToUpdate == null)
                    return NotFound($"Admin with Id = {adminId} not found");

                if (!adminToUpdate.Id.Equals(adminId))
                    return BadRequest("IDs don't correspond");

                if (!adminToUpdate.Name.ToLower().Equals(dto.Name.ToLower()))
                    if (this.NameAlreadyTaken(dto))
                        return BadRequest($"Title \"{dto.Name}\" has been already taken");

                return Ok(await _adminService.UpdateEntity(dto, adminToUpdate));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        private bool AdminCanNotBeUpdated(AdminDTO dto, Guid adminId)
        {
            IAdmin equalAdmin = this._adminService
                                .FindFirstBy(dbAdmin => 
                                                new ValueTask<bool>(dbAdmin.Name.ToLower().Equals(dto.Name.ToLower()))).Result;
            if (equalAdmin == null)
                return false;
            if (equalAdmin.Id.Equals(adminId))
                return false;
            return true;
        }

    }
}
