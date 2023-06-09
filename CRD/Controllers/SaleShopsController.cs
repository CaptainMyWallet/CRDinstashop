using CRD.Interfaces;
using CRD.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    public class SaleShopsController : ApiBaseController
    {
        private readonly IHttpContextAccessor _httpContentAccessor;

        private readonly ISaleShopsService _saleShopsService;
        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(SaleShopsController));
        public SaleShopsController(IConfiguration configuration, IHttpContextAccessor httpContentAccessor, ISaleShopsService saleShopsService)
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._httpContentAccessor = httpContentAccessor;
            _saleShopsService = saleShopsService;
        }
        
        [AllowAnonymous]
        [HttpGet("")]
        [ProducesResponseType(typeof(List<SaleShopModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAsync(int skip = 0, int take = 15, string FilterTitle = "", bool orderByDesc = true)
        {
            try
            {
                var data = await _saleShopsService.GetAsync(skip, take, FilterTitle, orderByDesc);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        [HttpGet("GetSaleShopByID/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SaleShopModel), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var data = await _saleShopsService.GetByIdAsync(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        [HttpPost("CreateSaleShop")]
        [ProducesResponseType(typeof(SaleShopModel), StatusCodes.Status200OK)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] SaleShopRequest model)
        {
            try
            {
                var data = await _saleShopsService.CreateAsync(model);

                return StatusCode(StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        [HttpPut("UpdateSaleShop")]
        [ProducesResponseType(typeof(SaleShopModel), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync( [FromBody] SaleShopRequest model)
        {
            try
            {
               var data = await _saleShopsService.UpdateAsync(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());

                return BadRequest();
            }
        }

        [HttpDelete("DeleteSaleShop/{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                await _saleShopsService.DeleteAsync(id);

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
