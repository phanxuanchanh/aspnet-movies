using Data.DAL;
using Data.DTO;
using System;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PaymentInfoBLL : BusinessLogicLayer
    {
        private bool disposed;

        public PaymentInfoBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public PaymentInfoBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        private PaymentInfoDTO ToPaymentInfoDTO(PaymentInfo paymentInfo)
        {
            if (paymentInfo == null)
                return null;

            PaymentInfoDTO paymentInfoDTO = new PaymentInfoDTO
            {
                paymentMethodId = paymentInfo.paymentMethodId,
                userId = paymentInfo.userId,
                cardNumber = paymentInfo.cardNumber,
                cvv = paymentInfo.cvv,
                owner = paymentInfo.owner,
                expirationDate = paymentInfo.expirationDate
            };

            if (includeTimestamp)
            {
                paymentInfoDTO.createAt = paymentInfo.createAt;
                paymentInfoDTO.updateAt = paymentInfo.updateAt;
            }

            return paymentInfoDTO;
        }

        private PaymentInfo ToPaymentInfo(PaymentInfoCreation paymentInfoCreation)
        {
            if (paymentInfoCreation == null)
                throw new Exception("@'paymentInfoCreation' must not be null");

            return new PaymentInfo
            {
                paymentMethodId = paymentInfoCreation.paymentMethodId,
                userId = paymentInfoCreation.userId,
                cardNumber = paymentInfoCreation.cardNumber,
                cvv = paymentInfoCreation.cvv,
                owner = paymentInfoCreation.owner,
                expirationDate = paymentInfoCreation.expirationDate,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        private PaymentInfo ToPaymentInfo(PaymentInfoUpdate paymentInfoUpdate)
        {
            if (paymentInfoUpdate == null)
                throw new Exception("@'paymentInfoUpdate' must not be null");

            return new PaymentInfo
            {
                paymentMethodId = paymentInfoUpdate.paymentMethodId,
                userId = paymentInfoUpdate.userId,
                cardNumber = paymentInfoUpdate.cardNumber,
                cvv = paymentInfoUpdate.cvv,
                owner = paymentInfoUpdate.owner,
                expirationDate = paymentInfoUpdate.expirationDate,
                updateAt = DateTime.Now,
            };
        }

        public async Task<CreationState> CreatePaymentInfoAsync(PaymentInfoCreation paymentInfoCreation)
        {
            PaymentInfo paymentInfo = ToPaymentInfo(paymentInfoCreation);
            if (
                paymentInfo.paymentMethodId <= 0 || string.IsNullOrEmpty(paymentInfo.userId)
                || string.IsNullOrEmpty(paymentInfo.cardNumber) || string.IsNullOrEmpty(paymentInfo.cvv)
                || string.IsNullOrEmpty(paymentInfo.owner) || string.IsNullOrEmpty(paymentInfo.expirationDate)
            )
            {
                throw new Exception("");
            }

            int checkExists = (int)await db.PaymentInfos
                .CountAsync(p => p.userId == paymentInfo.userId && p.paymentMethodId == paymentInfo.paymentMethodId);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected = await db.PaymentInfos.InsertAsync(paymentInfo);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        //public async Task<UpdateState> UpdatePaymentInfoAsync(PaymentInfoUpdate paymentInfoUpdate)
        //{
        //    if (dataAccessLevel == DataAccessLevel.User)
        //        throw new Exception("");
        //    PaymentInfo paymentInfo = ToPaymentInfo(paymentInfoUpdate);
        //    if (PaymentInfo.name == null)
        //        throw new Exception("");

        //    int affected;
        //    if (PaymentInfo.description == null)
        //        affected = await db.Categories.UpdateAsync(
        //            PaymentInfo,
        //            p =>new { p.name, p.updateAt },
        //            p =>p.ID == PaymentInfo.ID
        //        );
        //    else
        //        affected = await db.Categories.UpdateAsync(
        //            PaymentInfo,
        //            p =>new { p.name, p.description, p.updateAt },
        //            p =>p.ID == PaymentInfo.ID
        //        );

        //    return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        //}

        public async Task<DeletionState> DeletePaymentInfoAsync(string userId, int paymentMethodId)
        {
            if (string.IsNullOrEmpty(userId) || paymentMethodId <= 0)
                throw new Exception("");

            int affected = await db.PaymentInfos.DeleteAsync(p => p.paymentMethodId == paymentMethodId && p.userId == userId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
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
