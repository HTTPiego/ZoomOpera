using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperasController : ControllerBase
    {
        private readonly IService<IOpera, OperaDTO> _operaService;

        private readonly IService<IOperaImage, OperaImageDTO> _operaImageService;
        public OperasController(IService<IOpera, OperaDTO> operaService,
                                IService<IOperaImage, OperaImageDTO> operaImageService)
        {
            _operaService = operaService;
            _operaImageService = operaImageService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<IOpera>>> GetAllOperas()
        {
            try
            {
                return Ok(await _operaService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IOpera?>> GetOperaByLocationId([FromQuery] Guid locationId)
        {
            try
            {
                var dbOpera = await _operaService.FindFirstBy(o => new ValueTask<bool>(o.LocationId.Equals(locationId)));

                return Ok(dbOpera);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{operaId}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IOpera>> GetOperaById(Guid operaId)
        {
            try
            {
                var result = await _operaService.GetEntity(operaId);

                if (result == null)
                    return NotFound($"Opera with Id = {operaId} not found");

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
        public async Task<ActionResult<IOpera>> CreateOpera([FromBody] OperaDTO dto)
        {
            try
            {
                if (OperaIsNotValid(dto))
                    return BadRequest("Opera is not valid");

                if (this.TwoEqualOperas(dto))
                    return BadRequest("A similar Opera has been already registered");

                if (this._operaService.FindFirstBy(opera => new ValueTask<bool>(opera.LocationId.Equals(dto.LocationId))).Result != null)
                    return BadRequest("An Opera has been alredy placed in this location");

                var addedOpera = await _operaService.AddEntity(dto);
                
                dto.OperaImage.OperaId = addedOpera.Id;
                await _operaImageService.AddEntity(dto.OperaImage);                  

                return Ok(addedOpera);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Opera record");
            }
        }

        //        if (dto == null)
        //            return BadRequest("Opera is not valid");

        //        if (this.TwoEqualOperas(dto))
        //            return BadRequest("A similar Opera has been already registered");

        //        if (this._operaService.FindFirstBy(opera => opera.Location.Equals(dto.LocationId)).Result != null)
        //            return BadRequest("An Opera has been alredy placed in this location");

        //        return Ok(await _operaService.AddEntity(dto));

        private bool OperaIsNotValid(OperaDTO dto)
        {
            if (dto == null)
                return true;
            if (dto.Name == null
                || dto.ItalianDescription == null
                || dto.AuthorFirstName == null
                || dto.AuthorLastName == null
                || dto.OperaImage == null)
                return true;
            return false;

        }
        
        private bool TwoEqualOperas(OperaDTO dto)
        {
            return this._operaService.FindFirstBy(opera => new ValueTask<bool>(opera.Equals(dto))).Result != null;
        }

        [HttpPut("{operaId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IOpera>> UpdateOpera([FromBody] OperaDTO dto, Guid operaId)
        {
            try
            {
                if (OperaIsNotValid(dto))
                    return BadRequest("Opera is not valid");

                if (this.OperaCanNotBeUpdated(dto, operaId))
                    return BadRequest("A similar Opera has been already registered");

                var operaToUpdate = await _operaService.GetEntity(operaId);

                if (operaToUpdate == null)
                    return NotFound($"Opera with Id = {operaId} not found");

                if (!operaToUpdate.Id.Equals(operaId))
                    return BadRequest("IDs don't correspond");

                var updatedOpera = await _operaService.UpdateEntity(dto, operaToUpdate);
                
                await this._operaImageService.UpdateEntity(dto.OperaImage, updatedOpera.Image);

                return Ok(updatedOpera);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        private bool OperaCanNotBeUpdated(OperaDTO dto, Guid operaId)
        {
            var equalOpera = this._operaService.FindFirstBy(o => new ValueTask<bool>(o.EqualsTo(dto)));
            if (equalOpera == null)
                return false;
            if (equalOpera.Id.Equals(operaId))
                return false;
            return true;
        }

        [HttpDelete("{operaId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IOpera>> DeleteOpera(Guid operaId)
        {
            try
            {
                var operaToDelete = await _operaService.GetEntity(operaId);

                if (operaToDelete == null)
                {
                    return NotFound($"Opera with Id =  {operaId} not found");
                }

                return Ok(await _operaService.DeleteEntity(operaId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }


    }
}
