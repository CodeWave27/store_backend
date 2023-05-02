using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_backend.Authorization;
using store_backend.Context;
using store_backend.Dao;
using store_backend.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace store_backend.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DbController : ControllerBase
    {
        ApplicationContext DbContext;

        private readonly ILogger<DbController> _logger;

        public DbController(ILogger<DbController> logger, ApplicationContext db)
        {
            _logger = logger;
            DbContext = db;
        }

        /// <summary>
        /// This method create the database and insert the stablished data
        /// </summary>
        /// <response code="201">Returns that the db was created and the data inserted</response>
        /// <response code="400">If the the db and data cannot be inserted</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("CreateDb")]
        public IActionResult createDb()
        {
            
            return Ok(DbContext.Database.EnsureCreated());
        }

    }
}