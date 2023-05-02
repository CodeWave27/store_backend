using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using store_backend.Authorization;
using store_backend.Dao;
using store_backend.Dto;
using store_backend.Enums;
using System.Data;
using System.Text.Json;

namespace store_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController: ControllerBase
    {
        private IPersonaDao _personaDao;

        public PersonaController(IPersonaDao userService)
        {
            _personaDao = userService;
        }

        /// <summary>
        /// This method authenticate the person and returned basical info such as name, role and bearer token for authorization in the requests
        /// </summary>
        /// <response code="200">Return the user id, name, role and token</response>
        /// <response code="400">If the username or password are incorrects or the user doesn't exist in the database</response>
        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(AuthenticateResponseDto), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public IActionResult Authenticate(AuthenticateRequestDto model)
        {
            var response = _personaDao.Authenticate(model);
            return Ok(response);
        }

        /// <summary>
        /// This method returns all users info
        /// </summary>
        /// <response code="200">Returns all the user id, name, role and username</response>
        /// <response code="400">Unauthorized error token or not enough permissions</response>

        [Authorize(Roles.AdministradorSeguridad, Roles.AdministradorPuntoVenta)]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonaDTO>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public IActionResult GetAll()
        {
            var users= _personaDao.GetAll();

            return Ok(users);
        }

        /// <summary>
        /// This method return all user info according id provided
        /// </summary>
        /// <response code="200">Returns all the request user info such as id, name, role and username</response>
        /// <response code="400">Unauthorized error token or not enough permissions</response>

        [Authorize(Roles.AdministradorPuntoVenta, Roles.AdministradorInventario, Roles.AdministradorCompras, Roles.AdministradorSeguridad, Roles.AdministradorEstados)]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PersonaDTO), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public IActionResult GetById(int id)
        {
            // only admins can access other user records
            var currentUser = (PersonaDTO)HttpContext.Items["Persona"];
            if (id != currentUser.Id && (currentUser.Role != Roles.AdministradorPuntoVenta || currentUser.Role != Roles.AdministradorInventario || currentUser.Role != Roles.AdministradorCompras || currentUser.Role != Roles.AdministradorSeguridad || currentUser.Role != Roles.AdministradorEstados))
                return Unauthorized(new { message = "Unauthorized" });

            var user = _personaDao.GetById(id);
            return Ok(user);
        }

        /// <summary>
        /// This method create a new user according the info provided
        /// </summary>
        /// <response code="200">Returns all the request user info such as id, name, role and username</response>
        /// <response code="400">Unauthorized error token or not enough permissions</response>

        [Authorize(Roles.AdministradorPuntoVenta)]
        [HttpPost("")]
        [ProducesResponseType(typeof(Dictionary<string, dynamic>), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<IActionResult> CreateUser(PersonaDTO user)
        {
            try
            {
                var newUser = _personaDao.CreateUser(user);

                return StatusCode(201,new { new_user= user, message="User successfully created" }) ;
            } catch(Exception ex)
            {
                return StatusCode(400, new { message = "Unexpected error" });
            }
        }
    }
}
