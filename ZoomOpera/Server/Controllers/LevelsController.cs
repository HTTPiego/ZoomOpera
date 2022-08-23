using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.Shared.Entities;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private IService<ILevel, LevelDTO> _levelService;
        public LevelsController(IService<ILevel, LevelDTO> levelService)
        {
            _levelService = levelService;
        }

        [HttpGet("{idLevel}")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<ILevel>> GetLevelById(Guid idLevel)
        {
            try
            {
                var result = await _levelService.GetEntity(idLevel);

                if (result == null)
                    return NotFound($"Level with Id = {idLevel} not found");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<ILevel>>> GetLevelsByBuildingId([FromQuery] Guid buildingId)
        {
            try
            {
                return Ok(await _levelService.FindAllBy(l => new ValueTask<bool>( l.BuildingId.Equals(buildingId)) ));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "MonitorPlatform,Admin")]
        public async Task<ActionResult<IEnumerable<ILevel>>> GetAllLevels()
        {
            try
            {
                return Ok(await _levelService.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        //TODO: aggiungere controllo sull ID del building 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILevel>> AddLevel([FromBody] LevelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Level is not valid");

                if (this.TwoEqualLeves(dto))
                    return BadRequest($"A similar Level has been already registered");

                return Ok(await _levelService.AddEntity(dto));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new level record");
            }
        }


        private bool TwoEqualLeves(LevelDTO dto)
        {
            return this._levelService.FindFirstBy(dbLevel => new ValueTask<bool>( dbLevel.EqualsTo(dto))).Result != null;
        }

        [HttpPut("{idLevel}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILevel>> UpdateLevel([FromBody] LevelDTO dto, Guid idLevel)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Level is not valid");

                //if (await this.LevelCanNotBeUpdated(dto, idLevel))
                //    return BadRequest($"A similar Level has been already registered");

                var levelToUpdate = await _levelService.GetEntity(idLevel);

                if (levelToUpdate == null)
                    return NotFound($"Level with Id = {idLevel} not found");

                if (!levelToUpdate.Id.Equals(idLevel))
                    return BadRequest("IDs don't correspond");

                return Ok(await _levelService.UpdateEntity(dto, levelToUpdate));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        private async Task<bool> LevelCanNotBeUpdated(LevelDTO dto, Guid levelId)
        {
            var equalLevel = await this._levelService
                                    .FindFirstBy(l => new ValueTask<bool>(l.EqualsTo(dto)));
            //if (equalLevel == null)
            //    return true;
            if (equalLevel.Id.Equals(levelId))
                return false;
            return true;
        }

        [HttpDelete("{idLevel}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ILevel>> DeleteLevel(Guid idLevel)
        {
            try
            {
                var levelToDelete = await _levelService.GetEntity(idLevel);

                if (levelToDelete == null)
                {
                    return NotFound($"Level with Id = {idLevel} not found");
                }

                return Ok(await _levelService.DeleteEntity(idLevel));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }

        }

        

    }
}
