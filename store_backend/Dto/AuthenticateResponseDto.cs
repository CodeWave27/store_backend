using store_backend.Enums;

namespace store_backend.Dto
{
    public class AuthenticateResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Roles Role { get; set; }
        public string Token { get; set; }

        public AuthenticateResponseDto(PersonaDTO persona, string token)
        {
            Id= persona.Id;
            Name= persona.Name;
            Role= persona.Role;
            Token = token;
        }

    }
}
