using Data.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class PaymentMethodMigration
    {
        public static List<PaymentMethod> AddData()
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>();
            paymentMethods.Add(new PaymentMethod { name = "Visa", createAt = DateTime.Now, updateAt = DateTime.Now });
            paymentMethods.Add(new PaymentMethod { name = "Mastercard", createAt = DateTime.Now, updateAt = DateTime.Now });
            return paymentMethods;
        }

        public static void Migrate()
        {
            using (DBContext db = new DBContext())
            {
                long recordNumber = db.PaymentMethods.Count();
                if(recordNumber == 0)
                {
                    List<PaymentMethod> paymentMethods = AddData();

                    foreach (PaymentMethod paymentMethod in paymentMethods)
                    {
                        int affected = db.PaymentMethods.Insert(paymentMethod, new List<string> { "ID" });
                        if (affected == 0)
                            break;
                    }
                }
            }
        }

        public static async Task MigrateAsync()
        {
            using (DBContext db = new DBContext())
            {
                long recordNumber = await db.PaymentMethods.CountAsync();
                if (recordNumber == 0)
                {
                    List<PaymentMethod> paymentMethods = AddData();

                    foreach (PaymentMethod paymentMethod in paymentMethods)
                    {
                        int affected = await db.PaymentMethods.InsertAsync(paymentMethod, new List<string> { "ID" });
                        if (affected == 0)
                            break;
                    }
                }
            }
        }
    }
}
