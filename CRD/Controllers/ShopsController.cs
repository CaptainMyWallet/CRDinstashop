using CRD.Interfaces;
using CRD.Utils;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    public class ShopsController : ApiBaseController
    {
        private readonly IHttpContextAccessor _httpContentAccessor;

        private readonly IShopsService _shopsService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(ShopsController));
        public ShopsController(IConfiguration configuration, IHttpContextAccessor httpContentAccessor, IShopsService shopsService)
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._httpContentAccessor = httpContentAccessor;
            _shopsService = shopsService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<Shop>), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAsync([FromQuery] int skip = 0, int take = 15, string q = "", bool orderByDesc = true, [FromQuery] int[] categoryIds = null, [FromQuery] int[] tagIds = null)
        {
            try
            {
                var data = await _shopsService.GetAsync(skip, take, q, orderByDesc, categoryIds, tagIds);

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
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Shop), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var data = await _shopsService.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(SaleShopModel), StatusCodes.Status200OK)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] ShopRequest model)
        {
            try
            {
                var data = await _shopsService.CreateAsync(model);

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
        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SaleShopModel), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ShopRequest model)
        {
            try
            {
                model.Id = id;

                var data = await _shopsService.UpdateAsync(model);

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
        [HttpDelete("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteAsyncAsync([FromRoute] int id)
        {
            try
            {
                await _shopsService.DeleteAsync(id);

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
