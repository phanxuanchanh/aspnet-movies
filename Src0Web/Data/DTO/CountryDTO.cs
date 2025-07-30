using System;

namespace Data.DTO
{
    public class CountryDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCountryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCountryDto : CreateCountryDto
    {
        public int ID { get; set; }
    }
}
