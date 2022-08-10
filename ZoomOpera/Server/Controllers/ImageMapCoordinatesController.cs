using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMapCoordinatesController : ControllerBase
    {
        private readonly IService<IImageMapCoordinate, ImageMapCoordinateDTO> _service;

        public ImageMapCoordinatesController(IService<IImageMapCoordinate, ImageMapCoordinateDTO> service)
        {
            _service = service;
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IImageMapCoordinate>>> GetAllImageMapCoordinates()
        {
            try
            {
                return Ok(await _service.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IImageMapCoordinate>>> GetAllCoordinatesBy([FromQuery] Guid imageMapId)
        {
            try
            {
                var dbImageMapCoordinates = await _service.FindAllBy(c => new ValueTask<bool>(c.ImageMapId.Equals(imageMapId)));

                return Ok(dbImageMapCoordinates);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{imageMapCoordinateId}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IImageMapCoordinate>> GetImageMapCoordinateBy(Guid imageMapCoordinateId)
        {
            try
            {   
                var result = await _service.GetEntity(imageMapCoordinateId);

                if (result == null)
                    return NotFound($"Image Map Coordinate with Id = {imageMapCoordinateId} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IImageMapCoordinate>> AddImageMapCoordinate([FromBody] ImageMapCoordinateDTO dto)
        //{   
        //    try
        //    {
        //        if (dto == null)
        //            return BadRequest("Image Map Coordinate is not valid");

        //        if (TwoEqualsCoordinates(dto))
        //            return BadRequest
        //                ("A coordinate {x: " + dto.X + "; y: " + dto.Y + "} on Image Map with ID = \"" + dto.ImageMapId + "\" has been already setted" );

        //        return Ok(await _service.AddEntity(dto));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error creating new Opera's Image record");
        //    }
        //}

        private bool TwoEqualsCoordinates(ImageMapCoordinateDTO dto)
        {
            return _service.FindFirstBy(c => new ValueTask<bool>(c.Equals(dto))) != null;
        }

        //No Put API

        [HttpDelete("{imageMapCoordinateId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IImageMapCoordinate>> DeleteImageMapCoordinate(Guid imageMapCoordinateId)
        {
            try
            {
                var coordinateToDelete = await _service.GetEntity(imageMapCoordinateId);

                if (coordinateToDelete == null)
                    return NotFound($"Image Map Coordinate with Id =  {imageMapCoordinateId} not found");

                return Ok(await _service.DeleteEntity(imageMapCoordinateId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
