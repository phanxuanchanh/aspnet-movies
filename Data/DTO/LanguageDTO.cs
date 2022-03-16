using System;

namespace Data.DTO
{
    public class LanguageInfo
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class LanguageCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class LanguageUpdate : LanguageCreation
    {
        public int ID { get; set; }
    }
}
