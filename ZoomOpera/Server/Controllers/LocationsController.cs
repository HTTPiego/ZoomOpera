using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IService<ILocation, LocationDTO> _locationService;
        public LocationsController(IService<ILocation, LocationDTO> locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("{locationId}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<ILocation>> GetLocationById(Guid locationId)
        {
            try
            {
                var result = await _locationService.GetEntity(locationId);

                if (result == null)
                    return NotFound($"Location with Id = {locationId} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<ILocation>>> GetAllLocations()
        {
            try
            {
                return Ok(await _locationService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<ILocation>>> GetLocationsByLevelId([FromQuery] Guid levelId)
        {
            try
            {
                return Ok(await _locationService.FindAllBy(l => new ValueTask<bool>( l.LevelId.Equals(levelId) )));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILocation>> AddLocation([FromBody] LocationDTO dto)
        {
            try
            {
                if (LocationIsNotValid(dto))
                    return BadRequest("Location is not valid");

                if (this.TwoEqualLocations(dto))
                    return BadRequest($"A similar Location has been already registered");

                return Ok(await _locationService.AddEntity(dto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new location record");
            }
        }

        private bool LocationIsNotValid(LocationDTO dto)
        {
            if (dto == null)
                return true;
            if (dto.LocationCode == null
                || dto.Notes == null)
                return true;
            return false;
        }

        private bool TwoEqualLocations(LocationDTO location)
        {
            return this._locationService.FindFirstBy(dbLocation => new ValueTask<bool>( dbLocation.EqualsTo(location))).Result != null;
        }

        [HttpPut("{locationId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILocation>> UpdateLocation([FromBody] LocationDTO dto, Guid locationId)
        {
            try
            {
                if (LocationIsNotValid(dto))
                    return BadRequest("Location is not valid");

                //if (this.LocationCanNotBeUpdated(dto, locationId))
                //    return BadRequest($"A similar Location has been already registered");

                var locationToUpdate = await _locationService.GetEntity(locationId);

                if (locationToUpdate == null)
                    return NotFound($"Location with Id = {locationId} not found");

                if (!locationToUpdate.Id.Equals(locationId))
                    return BadRequest("IDs don't correspond");

                return Ok(await _locationService.UpdateEntity(dto, locationToUpdate));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        private bool LocationCanNotBeUpdated(LocationDTO dto, Guid locationId)
        {
            var equalLocation = this._locationService.FindFirstBy(l => new ValueTask<bool>(l.EqualsTo(dto)));
            if (equalLocation == null)
                return false;
            if (equalLocation.Id.Equals(locationId))
                return false;
            return true;
        }


        [HttpDelete("{idLocation}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILocation>> DeleteLocation(Guid idLocation)
        {
            try
            {
                var locationToDelete = await _locationService.GetEntity(idLocation);

                if (locationToDelete == null)
                {
                    return NotFound($"Location with Id = {idLocation} not found");
                }

                return Ok(await _locationService.DeleteEntity(idLocation));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
