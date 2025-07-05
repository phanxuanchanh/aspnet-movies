using System;

namespace Data.DAL
{
    public class PaymentMethod
    {
        public int ID { get; set; }
        public string name { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
