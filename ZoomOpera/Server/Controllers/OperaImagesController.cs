using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperaImagesController : ControllerBase
    {
        private readonly IService<IOperaImage, OperaImageDTO> _service;

        public OperaImagesController(IService<IOperaImage, OperaImageDTO> service)
        {
            _service = service;
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IOperaImage>>> GetAllOperaImages()
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
        public async Task<ActionResult<IOperaImage?>> GetOperaImageByOperaId([FromQuery] Guid operaId)
        {
            try
            {
                var dbOperaImage = await _service.FindFirstBy(o => new ValueTask<bool>(o.OperaId.Equals(operaId)));

                return Ok(dbOperaImage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{operaImageId}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IOperaImage>> GetOperaImageById(Guid operaImageId)
        {
            try
            {
                var result = await _service.GetEntity(operaImageId);

                if (result == null)
                    return NotFound($"Opera's Image with Id = {operaImageId} not found");

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
        //public async Task<ActionResult<IOperaImage>> AddOperaImage([FromBody] OperaImageDTO dto)
        //{
        //    try
        //    {
        //        if (dto == null || dto.Image == null)
        //            return BadRequest("Opera's Image is not valid");

        //        if (OneOperaImageHasBeenAlreadySetted(dto))
        //            return BadRequest("An Opera's Image has been already setted");

        //        return Ok(await _service.AddEntity(dto));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error creating new Opera's Image record");
        //    }
        //}

        private bool OneOperaImageHasBeenAlreadySetted(OperaImageDTO dto)
        {
            return _service.FindFirstBy(i => new ValueTask<bool>(i.OperaId.Equals(dto.OperaId))) != null;
        }

        //[HttpPut("{operaImageId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IOperaImage>> UpdateOperaImage([FromBody] OperaImageDTO dto, Guid operaImageId)
        //{
        //    try
        //    {
        //        if (dto == null ||  dto.Image == null)
        //            return BadRequest("Opera's Image is not valid");

        //        var operaImageToUpdate = await _service.GetEntity(operaImageId);

        //        if (operaImageToUpdate == null)
        //            return NotFound($"Opera's Image with Id = {operaImageId} not found");

        //        if (!operaImageToUpdate.Id.Equals(operaImageId))
        //            return BadRequest("IDs don't correspond");

        //        return Ok(await _service.UpdateEntity(dto, operaImageToUpdate));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error updating data");
        //    }
        //}

        //[HttpDelete("{operaImageId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IOperaImage>> DeleteOperaImage(Guid operaImageId)
        //{
        //    try
        //    {
        //        var operaImageToDelete = await _service.GetEntity(operaImageId);

        //        if (operaImageToDelete == null)
        //            return NotFound($"Opera with Id =  {operaImageId} not found");

        //        return Ok(await _service.DeleteEntity(operaImageId));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error deleting data");
        //    }
        //}

    }
}
