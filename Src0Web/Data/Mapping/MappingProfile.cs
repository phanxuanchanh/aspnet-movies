using Data.DTO;
using Data.Models;
using System;
using Web.Shared.Mapper;

namespace Data.Mapping
{
    public class MappingProfile : IMappingProfile
    {
        public void Configure(Mapper mapper)
        {
            mapper.CreateMap<Taxonomy, CategoryDto, Taxonomy_CategoryDto>();
            mapper.CreateMap<Role, RoleDto>(AutoMapperHelper.AutoMap<Role, RoleDto>());
        }
    }
}
