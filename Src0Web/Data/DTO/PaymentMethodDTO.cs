using System;

namespace Data.DTO
{
    public class PaymentMethodDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreatePaymentMethodDto
    {
        public string Name { get; set; }
    }

    public class UpdatePaymentMethodDto : CreatePaymentMethodDto
    {
        public int ID { get; set; }
    }
}