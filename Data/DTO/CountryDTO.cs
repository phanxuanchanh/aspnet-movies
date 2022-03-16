using System;

namespace Data.DTO
{
    public class CountryInfo
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class CountryCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CountryUpdate : CountryCreation
    {
        public int ID { get; set; }
    }
}
