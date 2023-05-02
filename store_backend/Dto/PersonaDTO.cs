using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using store_backend.Enums;

namespace store_backend.Dto
{
    public class PersonaDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }
        public string PasswordHash { get; set; }
    }
}
