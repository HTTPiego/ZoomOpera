using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorPlatformsController : ControllerBase
    {

        private readonly IService<IMonitorPlatform, MonitorPlatformDTO> _platformService;

        public MonitorPlatformsController(IService<IMonitorPlatform, MonitorPlatformDTO> platformService)
        {
            _platformService = platformService;
        }


        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IMonitorPlatform>>> GetAllPlatforms()
        {
            try
            {
                return Ok(await _platformService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IMonitorPlatform>>> GetPlatformsByLevelId([FromQuery] Guid levelId)
        {
            try
            {
                return Ok(await _platformService.FindAllBy( p => new ValueTask<bool>( p.LevelId.Equals(levelId)) ));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{idPlatform}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IMonitorPlatform>> GetPlatformById(Guid idPlatform)
        {
            try
            {
                var result = await _platformService.GetEntity(idPlatform);

                if (result == null)
                    return NotFound($"Platform with Id = {idPlatform} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpDelete("{idPlatform}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IMonitorPlatform>> DeletePlatform(Guid idPlatform)
        {
            try
            {
                var locationToDelete = await _platformService.GetEntity(idPlatform);

                if (locationToDelete == null)
                {
                    return NotFound($"Platform with Id =  {idPlatform} not found");
                }

                return Ok(await _platformService.DeleteEntity(idPlatform));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IMonitorPlatform>> AddPlatform([FromBody] MonitorPlatformDTO dto)
        {
            try
            {
                if (MonitorPlatformIsNotValid(dto))
                    return BadRequest("Monitor Platform is not valid");

                if (this.NameAreadyTaken(dto))
                    return BadRequest($"The name {dto.Name} has been already taken");

                if (this.TwoEqualPlatforms(dto))
                    return BadRequest("A similar Platform has been already registered");

                return Ok(await _platformService.AddEntity(dto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Platform record");
            }
        }

        private bool MonitorPlatformIsNotValid(MonitorPlatformDTO dto)
        {
            if (dto == null)
                return true;
            if (dto.MonitorCode == null
                || dto.Name == null
                || dto.Password == null)
                return true;
            return false;
        }

        private bool NameAreadyTaken(MonitorPlatformDTO dto)
        {
            return this._platformService.FindFirstBy(platform =>
                    new ValueTask<bool>(platform.Name.ToLower().Equals(dto.Name.ToLower()))).Result != null;
        }

        private bool TwoEqualPlatforms(MonitorPlatformDTO platform)
        {
            return this._platformService.FindFirstBy(dbPlatform => new ValueTask<bool>( dbPlatform.EqualsTo(platform))).Result != null;
        }

        [HttpPut("{idPlatfrom}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IMonitorPlatform>> UpdatePlatform([FromBody] MonitorPlatformDTO dto, Guid idPlatfrom)
        {
            try
            {
                if (MonitorPlatformIsNotValid(dto))
                    return BadRequest("Monitor Platform is not valid");

                if (this.TwoEqualPlatforms(dto))
                    return BadRequest("A similar Platform has been already registered");

                var platformToUpdate = await _platformService.GetEntity(idPlatfrom);

                if (platformToUpdate == null)
                    return NotFound($"Platform with Id = {idPlatfrom} not found");

                if (!platformToUpdate.Id.Equals(idPlatfrom))
                    return BadRequest("IDs don't correspond");

                if (!platformToUpdate.Name.ToLower().Equals(dto.Name.ToLower()))
                    if (this.NameAreadyTaken(dto))
                        return BadRequest($"The name {dto.Name} has been already taken");

                return Ok(await _platformService.UpdateEntity(dto, platformToUpdate));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

    }
}
