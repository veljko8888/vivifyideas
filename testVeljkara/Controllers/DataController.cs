using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.Controllers
{
    [ApiController]
    [Route("v1/diff")]
    public class DataController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DataController> _logger;
        
        private readonly IDataService _dataService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public DataController(
            ILogger<DataController> logger,
            ApplicationDbContext dbContext,
            IDataService dataService,
            IUserService userService,
            IAuthService authService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _dataService = dataService;
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Saves base64 data for left side
        /// </summary>
        /// <param name="id">id of the base64 data record</param>
        /// <param name="data">string representation of the base64 data</param>
        /// <returns>http response with eventual data info</returns>
        [Authorize]
        [HttpPut]
        [Route("{id}/left")]
        public async Task<IActionResult> SaveDataLeft(int id, [FromBody]DataDto data)
        {
            data.Id = id;
            data.Side = "left";
            Result<DataDto> insertedData = await _dataService.Insert(data);
            insertedData.ActionName = "SaveDataLeft";
            return GenerateResponse(insertedData);
        }

        /// <summary>
        /// Saves base64 data for left side
        /// </summary>
        /// <param name="id">id of the base64 data record</param>
        /// <param name="data">string representation of the base64 data</param>
        /// <returns>http response with eventual data info</returns>
        [Authorize]
        [HttpPut]
        [Route("{id}/right")]
        public async Task<IActionResult> SaveDataRight(int id, [FromBody]DataDto data)
        {
            data.Id = id;
            data.Side = "right";
            Result<DataDto> insertedData = await _dataService.Insert(data);
            insertedData.ActionName = "SaveDataRight";
            return GenerateResponse(insertedData);
        }

        /// <summary>
        /// Gets the difference information between two base64 data records marked
        /// with the same id
        /// </summary>
        /// <param name="id">id of the base64 data record/records</param>
        /// <returns>http response with eventual data info</returns>
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetDiff(int id)
        {
            var differenceResult = await _dataService.CheckDiff(id);
            return GenerateResponse(differenceResult);
        }
    }
}
