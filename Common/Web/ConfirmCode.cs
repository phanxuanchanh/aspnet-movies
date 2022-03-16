using Common.Hash;
using Common.Mail;
using System;

namespace Common.Web
{
    public class ConfirmCode
    {
        public string Send(string emailAddress)
        {
            string confirmCode = new Random().NextStringOnlyNumericCharacter(8);
            string message = string.Format("Mã xác nhận của bạn là: {0}", confirmCode);
            new EMail().Send(emailAddress, "Mã xác nhận tài khoản", message);
            return confirmCode;
        }

        public string CreateToken(int length = 30)
        {
            string randomString = new Random().NextString(20);
            HashFunction hash = new HashFunction();
            return hash.PBKDF2_Hash(randomString, "confirmToken", length);
        }
    }
}