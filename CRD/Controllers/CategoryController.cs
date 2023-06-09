using CRD.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace CRD.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : ApiBaseController
    {
        private readonly ICategoryService _categoryService;


        private readonly IHttpContextAccessor _httpContentAccessor;

        private IConfiguration _configuration;
        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(CategoryController));
        public CategoryController(IConfiguration configuration, ICategoryService lcatrgoryService, IHttpContextAccessor httpContentAccessor)
            : base(configuration, httpContentAccessor)
        {
            this._configuration = configuration;
            this._categoryService = lcatrgoryService;
            this._httpContentAccessor = httpContentAccessor;
        }
        [HttpGet(nameof(categories))]
        [ProducesResponseType(typeof(List<CategoryDTOL>), StatusCodes.Status200OK)]

        public async Task<IActionResult> categories()
        {

            var result = await _categoryService.GetCategory();

            return JsonContent(result);
        }
    }
}
