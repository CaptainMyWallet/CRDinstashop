using CRD.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net;
using CRD.Utils;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    public class WeekShopController : ApiBaseController
    {
        private readonly IWeekShopService _weekShopService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(WeekShopController));

        public WeekShopController(IConfiguration configuration, IWeekShopService weekShopService, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpContextAccessor)
        {
            this._configuration = configuration;
            this._weekShopService = weekShopService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] WeekShop model)
        {
            var result = await _weekShopService.CreateAsync(model);
            if (result)
                return Ok();
            else
                return BadRequest("Unable to create weekshop");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WeekShop), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var weekShop = await _weekShopService.GetByIdAsync(id);
            if (weekShop != null)
                return Ok(weekShop);
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _weekShopService.DeleteAsync(id);
            if (result)
                return Ok();
            else
                return BadRequest("Unable to delete weekshop");
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<WeekShop>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int skip, int take, string q, bool orderByDesc)
        {
            var response = await _weekShopService.GetAsync(skip, take, q, orderByDesc);
            if (response != null)
                return Ok(response);
            else
                return NotFound();
        }

        [HttpPost(nameof(Update))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] WeekShop model)
        {
            var result = await _weekShopService.UpdateAsync(model);
            if (result)
                return Ok();
            else
                return BadRequest("Unable to create weekshop");
        }
    }
}
