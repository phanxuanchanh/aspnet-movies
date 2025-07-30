using System;

namespace Data.DTO
{
    public class RoleDto
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateRoleDto
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class UpdateRoleDto: CreateRoleDto
    {

    }
}
