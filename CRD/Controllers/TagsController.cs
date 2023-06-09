using CRD.Interfaces;
using CRD.Services;
using CRD.Utils;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : ApiBaseController
    {
        private readonly IHttpContextAccessor _httpContentAccessor;

        private readonly ITagsService _tagsService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(ShopsController));
        public TagsController(IConfiguration configuration, IHttpContextAccessor httpContentAccessor, ITagsService tagsService)
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._httpContentAccessor = httpContentAccessor;
            this._tagsService = tagsService;
        }

        [HttpGet("GetAsync")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<TagDTOT>), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAsync([FromQuery] int skip = 0, int take = 15, string q = "", bool orderByDesc = true)
        {
            try
            {
                var data = await _tagsService.GetAsync(skip, take, q, orderByDesc);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>        
        /// <response code="400">If error</response>
        [HttpGet("GetByIdAsync/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Shop), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var data = await _tagsService.GetByIdAsync(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        /// <response code="201"></response>        
        /// <response code="400">If error</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] Tag model)
        {
            try
            {
                var data = await _tagsService.CreateAsync(model);

                return StatusCode(StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>        
        /// <response code="400">If error</response>
        [HttpPut("UpdateAsync/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TagDTOT), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Tag model)
        {
            try
            {
                model.Id = id;

                var data = await _tagsService.UpdateAsync(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>        
        /// <response code="400">If error</response>
        [HttpDelete("DeleteAsyncAsync/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteAsyncAsync([FromRoute] int id)
        {
            try
            {
                await _tagsService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

    }
}
