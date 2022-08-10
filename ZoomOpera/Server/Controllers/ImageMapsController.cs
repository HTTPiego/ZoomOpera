using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMapsController : ControllerBase
    {
        private readonly IService<IImageMap, ImageMapDTO> _imageMapService;

        private readonly IService<IImageMapCoordinate, ImageMapCoordinateDTO> _coordinatesService;

        public ImageMapsController(IService<IImageMap, ImageMapDTO> imageMapService, 
                                    IService<IImageMapCoordinate, ImageMapCoordinateDTO> coordinatesService)
        {
            _imageMapService = imageMapService;
            _coordinatesService = coordinatesService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IImageMap>>> GetAllImageMaps()
        {
            try
            {
                return Ok(await _imageMapService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]    
        public async Task<ActionResult<IEnumerable<IImageMap?>>> GetAllImageMapsBy([FromQuery] Guid operaImageId)
        {
            try
            {
                var imageMaps = await _imageMapService.FindAllBy(m => new ValueTask<bool>(m.OperaImageId.Equals(operaImageId)));

                return Ok(imageMaps);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{imageMapId}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IImageMap>> GetImageMapId(Guid imageMapId)
        {   
            try
            {
                var result = await _imageMapService.GetEntity(imageMapId);

                if (result == null)
                    return NotFound($"Image Map with Id = {imageMapId} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IImageMap>> AddImageMap([FromBody] ImageMapDTO dto)
        {
            try
            {
                if (ImageMapIsNotValid(dto))
                    return BadRequest("Image Map is not valid");

                //if (ThereAreOverlappedImageMap())
                //    return BadRequest("The Image Map overlaps with another/others one/s");

                var addedImageMap = await _imageMapService.AddEntity(dto);

                dto.ImgeMapCoordinates.AsParallel().ForAll(c => 
                                                    { c.ImageMapId = addedImageMap.Id; 
                                                      _coordinatesService.AddEntity(c); });

                return Ok(addedImageMap);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Opera's Image record");
            }
        }

        private bool ImageMapIsNotValid(ImageMapDTO dto)
        {
            if (dto == null)
                return true;
            if (!Enum.IsDefined(dto.ImageMapShape)
                || dto.Title == null
                || dto.DetailedDescription == null
                || dto.ImgeMapCoordinates == null)
                return true;
            return false;
        }

        private bool ThereAreOverlappedImageMap()
        {
            return true;
        }

        //No Put API

        [HttpDelete("{imageMapId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IImageMap>> DeleteImageMap(Guid imageMapId)
        {
            try
            {
                var imageMapToDelete = await _imageMapService.GetEntity(imageMapId);

                if (imageMapToDelete == null)
                    return NotFound($"Image Map with Id =  {imageMapId} not found");

                return Ok(await _imageMapService.DeleteEntity(imageMapId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
