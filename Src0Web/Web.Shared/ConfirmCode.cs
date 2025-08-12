using Common.Hash;
using System;
using Web.Shared;

namespace Common.Web
{
    public class ConfirmCode
    {
        public string Send(string emailAddress)
        {
            string confirmCode = new Random().NextStringOnlyNumericCharacter(8);
            string message = string.Format("Mã xác nhận của bạn là: {0}", confirmCode);
            //new SmtpMailSender().Send(emailAddress, "Mã xác nhận tài khoản", message);
            return confirmCode;
        }

        public string CreateToken(int length = 30)
        {
            string randomString = new Random().NextString(20);
            return HashFunction.PBKDF2_Hash(randomString, "confirmToken", length);
        }
    }
}