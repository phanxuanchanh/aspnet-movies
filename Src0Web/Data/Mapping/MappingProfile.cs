using Data.DTO;
using Data.Models;
using Web.Shared.Mapper;

namespace Data.Mapping
{
    public class MappingProfile : IMappingProfile
    {
        public void Configure(Mapper mapper)
        {
            mapper.CreateMap<Taxonomy, CategoryDto, Taxonomy_CategoryDto>();
            mapper.CreateMap<Taxonomy, TagDto>(AutoMapperHelper.AutoMap<Taxonomy, TagDto>());
            mapper.CreateMap<FilmMetadata, LanguageDto>(AutoMapperHelper.AutoMap<FilmMetadata, LanguageDto>());
            mapper.CreateMap<FilmMetadata, CountryDto>(AutoMapperHelper.AutoMap<FilmMetadata, CountryDto>());
            mapper.CreateMap<Role, RoleDto>(AutoMapperHelper.AutoMap<Role, RoleDto>());
        }
    }
}
