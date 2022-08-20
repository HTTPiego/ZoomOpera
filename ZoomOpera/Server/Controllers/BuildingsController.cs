using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {

        private IService<IBuilding, BuildingDTO> _buildingService;

        public BuildingsController(IService<IBuilding, BuildingDTO> buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IBuilding>>> GetAllBuildings()
        {
            try
            {
                return Ok(await _buildingService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IBuilding>>> GetAllBuildingsBy([FromQuery] Guid firteringId)
        {
            try
            {
                return Ok(await this._buildingService.FindAllBy(b => new ValueTask<bool>( b.Id.Equals(firteringId))));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{idBuilding}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IBuilding>> GetBuildingById(Guid idBuilding)
        {
            try
            {
                var result = await _buildingService.GetEntity(idBuilding);

                if (result == null)
                    return NotFound($"Building with Id = {idBuilding} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpDelete("{idBuilding}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IBuilding>> DeleteBuilding(Guid idBuilding)
        {
            try
            {
                var buildingToDelete = await _buildingService.GetEntity(idBuilding);

                if (buildingToDelete == null)
                {
                    return NotFound($"Building with Id = {idBuilding} not found");
                }

                return Ok(await _buildingService.DeleteEntity(idBuilding));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IBuilding>> CreateBuilding([FromBody] BuildingDTO dto)
        {
            try
            {
                if (DtoIsNotValid(dto))
                    return BadRequest("Building is not valid");

                if (this.TwoEqualBuilding(dto))
                    return BadRequest("A similar building has been already registered");

                return Ok(await _buildingService.AddEntity(dto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new location record");
            }

        }

        private bool TwoEqualBuilding(BuildingDTO dto)
        {
            return this._buildingService.FindFirstBy(building => new ValueTask<bool>( building.EqualsTo(dto))).Result != null;
        }

        private bool DtoIsNotValid(BuildingDTO dto)
        {
            if (dto == null)
                return true;
            if (dto.Name == null
                || dto.BuildingCode == null)
                return true;
            return false;
        }

        [HttpPut("{idBuilding}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IBuilding>> UpdateBuilding([FromBody] BuildingDTO dto, Guid idBuilding)
        {
            try
            {
                if (DtoIsNotValid(dto))
                    return BadRequest("Building is not valid");

                //if (this.BuildingCanNotBeUpdated(dto, idBuilding))
                //    return BadRequest("A similar building has been already registered");

                var buildingToUpdate = await _buildingService.GetEntity(idBuilding);

                if (buildingToUpdate == null)
                    return NotFound($"Building with Id = {idBuilding} not found");

                if (!buildingToUpdate.Id.Equals(idBuilding))
                    return BadRequest("IDs don't correspond");

                return Ok(await _buildingService.UpdateEntity(dto, buildingToUpdate));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        private bool BuildingCanNotBeUpdated(BuildingDTO dto, Guid buildingId)
        {
            var equalBuilding = this._buildingService.FindFirstBy(building => new ValueTask<bool>(building.EqualsTo(dto))).Result;
            if (equalBuilding == null)
                return false;
            if (equalBuilding.Id.Equals(buildingId))
                return false;
            return true;
        }

    }
}
 