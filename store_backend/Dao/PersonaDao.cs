using Microsoft.Extensions.Options;

using store_backend.Authorization;
using store_backend.Context;
using store_backend.Dto;
using store_backend.Helpers;


namespace store_backend.Dao
{
    public class PersonaDao: IPersonaDao
    {
        private ApplicationContext _context;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public PersonaDao(
            ApplicationContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }


        public AuthenticateResponseDto Authenticate(AuthenticateRequestDto model)
        {
            var user = _context.Personas.SingleOrDefault(x => x.Username == model.Username);
            // validate
          
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful so generate jwt token
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponseDto(user, jwtToken);
        }

        public IEnumerable<Dictionary<string, dynamic>> GetAll()
        {
            var personas= _context.Personas;
            List<Dictionary<string, dynamic>> personasResponse= new List<Dictionary<string, dynamic>>();

            foreach(var persona in personas)
            {
                Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();

                item.Add("username", persona.Username);
                item.Add("role", persona.Role);
                item.Add("name", persona.Name);

                personasResponse.Add(item);
            }

            Console.WriteLine(personasResponse);

            return personasResponse;
        }

        public PersonaDTO GetById(int id)
        {
            var user = _context.Personas.Find(id);


            if (user == null) throw new KeyNotFoundException("Person not found");

            return user;
        }

        public Dictionary<string, dynamic> GetByIdRequest(int id)
        {
            var user = _context.Personas.Find(id);

            Dictionary<string, dynamic> userResponse= new Dictionary<string, dynamic>();

            if (user == null) throw new KeyNotFoundException("Person not found");

            userResponse.Add("id", user.Id);
            userResponse.Add("username", user.Username);
            userResponse.Add("name", user.Name);
            userResponse.Add("role", user.Role);

            return userResponse;
        }


        public async Task<int> CreateUser(PersonaDTO persona)
        {
            var user = persona;

            if(persona != null)
            {
                Console.WriteLine("b");
                //to hash password before insert in the database
                user.PasswordHash= BCrypt.Net.BCrypt.HashPassword(((PersonaDTO)persona).PasswordHash);

                _context.Add(user);

                return await _context.SaveChangesAsync();
            }
            Console.WriteLine("b");
            return 0;
        }
    }
    public interface IPersonaDao
    {
        AuthenticateResponseDto Authenticate(AuthenticateRequestDto model);
        IEnumerable<Dictionary<string, dynamic>> GetAll();
        PersonaDTO GetById(int id);
        Dictionary<string, dynamic> GetByIdRequest(int id);
        Task<int> CreateUser(PersonaDTO user);

    }
}
