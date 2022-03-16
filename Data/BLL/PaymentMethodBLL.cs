using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PaymentMethodBLL : BusinessLogicLayer
    {
        private bool disposed;

        public PaymentMethodBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public PaymentMethodBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        private PaymentMethodInfo ToPaymentMethodInfo(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                return null;

            PaymentMethodInfo paymentMethodInfo = new PaymentMethodInfo();
            paymentMethodInfo.ID = paymentMethod.ID;
            paymentMethodInfo.name = paymentMethod.name;

            if (includeTimestamp)
            {
                paymentMethodInfo.createAt = paymentMethod.createAt;
                paymentMethodInfo.updateAt = paymentMethod.updateAt;
            }

            return paymentMethodInfo;
        }

        private PaymentMethod ToPaymentMethod(PaymentMethodCreation paymentMethodCreation)
        {
            if (paymentMethodCreation == null)
                throw new Exception("@'paymentMethodCreation' must not be null");

            return new PaymentMethod
            {
                name = paymentMethodCreation.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private PaymentMethod ToPaymentMethod(PaymentMethodUpdate paymentMethodUpdate)
        {
            if (paymentMethodUpdate == null)
                throw new Exception("@'paymentMethodUpdate' must not be null");

            return new PaymentMethod
            {
                name = paymentMethodUpdate.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<PaymentMethodInfo>> GetPaymentMethodsAsync()
        {
            List<PaymentMethodInfo> paymentMethods = null;
            if (includeTimestamp)
                paymentMethods = (await db.PaymentMethods.ToListAsync())
                    .Select(p => ToPaymentMethodInfo(p)).ToList();
            else
                paymentMethods = (await db.PaymentMethods.ToListAsync(c => new { c.ID, c.name }))
                    .Select(p => ToPaymentMethodInfo(p)).ToList();

            return paymentMethods;
        }

        public async Task<PaymentMethodInfo> GetMethodInfoAsync(int paymemtMethodId)
        {
            if (paymemtMethodId <= 0)
                throw new Exception("@'paymentMethodId' must be greater than 0");

            PaymentMethod paymentMethod = null;
            if (includeTimestamp)
                paymentMethod = (await db.PaymentMethods.SingleOrDefaultAsync(p => p.ID == paymemtMethodId));
            else
                paymentMethod = (await db.PaymentMethods
                    .SingleOrDefaultAsync(p => new { p.ID, p.name }, p => p.ID == paymemtMethodId));

            return ToPaymentMethodInfo(paymentMethod);
        }

        public PagedList<PaymentMethodInfo> GetPaymentMethods(int pageIndex, int pageSize)
        {
            SqlPagedList<PaymentMethod> pagedList = null;
            Expression<Func<PaymentMethod, object>> orderBy = c => new { c.ID };
            if (includeTimestamp)
                pagedList = db.PaymentMethods.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.PaymentMethods.ToPagedList(
                    c => new { c.ID, c.name },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<PaymentMethodInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToPaymentMethodInfo(c)).ToList()
            };
        }

        public async Task<PagedList<PaymentMethodInfo>> GetPaymentMethodsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<PaymentMethod> pagedList = null;
            Expression<Func<PaymentMethod, object>> orderBy = c => new { c.ID };
            if (includeTimestamp)
                pagedList = await db.PaymentMethods.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.PaymentMethods.ToPagedListAsync(
                    c => new { c.ID, c.name },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<PaymentMethodInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToPaymentMethodInfo(c)).ToList()
            };
        }

        public async Task<CreationState> CreatePaymentMethodAsync(PaymentMethodCreation paymentMethodCreation)
        {
            PaymentMethod paymentMethod = ToPaymentMethod(paymentMethodCreation);
            if (paymentMethod.name == null)
                throw new Exception("@'paymentMethod.name' must not be null");

            int affected = await db.PaymentMethods.InsertAsync(paymentMethod);

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdatePaymentMethodAsync(PaymentMethodUpdate paymentMethodUpdate)
        {
            PaymentMethod paymentMethod = ToPaymentMethod(paymentMethodUpdate);
            if (paymentMethod.name == null)
                throw new Exception("@'paymentMethod.name' must not be null");

            int affected = await db.PaymentMethods
                .UpdateAsync(paymentMethod, p => new { p.name, p.updateAt }, p => p.ID == paymentMethod.ID);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeletePaymentMethodAsync(int paymentMethodId)
        {
            if (paymentMethodId <= 0)
                throw new Exception("@'paymentMethodId' must be greater than 0");

            long paymentInfoNumber = await db.PaymentInfos
                .CountAsync(p => p.paymentMethodId == paymentMethodId);
            if (paymentInfoNumber > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.PaymentMethods.DeleteAsync(p => p.ID == paymentMethodId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.PaymentMethods.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
