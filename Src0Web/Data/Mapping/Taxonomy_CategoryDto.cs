using Data.DAL;
using Data.DTO;
using Web.Shared.Mapper;

namespace Data.Mapping
{
    public class Taxonomy_CategoryDto : IMappingProfile<Taxonomy, CategoryDto>
    {
        public CategoryDto Map(Taxonomy source)
        {
            return new CategoryDto {
                ID = source.Id,
                Name = source.Name,
                Description = source.Description,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt
            };
        }
    }
}
