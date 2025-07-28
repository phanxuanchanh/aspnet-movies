using Data.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PaymentMethodDao
    {
        private readonly DBContext _context;

        public PaymentMethodDao(DBContext context)
            : base()
        {
            _context = context;
        }

        public async Task<List<PaymentMethod>> GetsAsync()
        {
            return await _context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethod> GetAsync(int id)
        {
            return await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> AddAsync(PaymentMethod paymentMethod)
        {
            paymentMethod.CreatedAt = DateTime.Now;
            return await _context.PaymentMethods
                .InsertAsync(paymentMethod, new List<string> { "ID", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(PaymentMethod paymentMethod)
        {
            paymentMethod.UpdatedAt = DateTime.Now;
            return await _context.PaymentMethods
                .Where(x => x.Id == paymentMethod.Id)
                .UpdateAsync(paymentMethod, p => new { p.Name, p.UpdatedAt });
        }

        public async Task<int> DeleteAsync(int id, bool forceDelete = false)
        {
            PaymentMethod method = await GetAsync(id);
            if (method == null)
                return 0;

            if (forceDelete)
                return await _context.PaymentMethods.DeleteAsync(x => x.Id == id);

            method.DeletedAt = DateTime.Now;
            return await _context.PaymentMethods
                .Where(x => x.Id == id)
                .UpdateAsync(method, s => new { s.DeletedAt });
        }
    }
}
