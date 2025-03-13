using Core.Entities;

namespace API.DTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRole? Role { get; set; }
    }
}