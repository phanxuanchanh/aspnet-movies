using System;

namespace Data.DTO
{
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool Activated { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public RoleDto Role { get; set; }
    }

    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public bool Activated { get; set; }
        public string RoleId { get; set; }
    }
}
