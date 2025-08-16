using Data.DAL;
using Data.DTO;
using Web.Shared.Mapper;

namespace Data.Mapping
{
    public class MappingProfile : IMappingProfile
    {
        public void Configure(Mapper mapper)
        {
            mapper.CreateMap<Taxonomy, CategoryDto, Taxonomy_CategoryDto>();
        }
    }
}
