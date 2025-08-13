using Data.Mapping;
using Web.Shared.Mapper;

namespace Web.App_Start
{
    public class MapperConfig
    {
        public static void RegisterProfiles(MapperService mapper)
        {
            mapper.Add(new Taxonomy_CategoryDto());
        }
    }
}