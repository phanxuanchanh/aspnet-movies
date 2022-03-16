using System;

namespace Data.DAL
{
    internal class PaymentInfo
    {
        public string userId { get; set; }
        public int paymentMethodId { get; set; }
        public string cardNumber { get; set; }
        public string cvv { get; set; }
        public string owner { get; set; }
        public string expirationDate { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
