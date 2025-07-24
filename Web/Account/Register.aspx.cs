using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private UserDao userBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            userBLL = new UserDao();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateDetail = null;
            try
            {
                InitValidation();

                if (CheckLoggedIn())
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    await SetDrdlPaymentMethod();
                    if (IsPostBack)
                    {
                        await RegisterAccount();
                    }
                }
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            userBLL.Dispose();
        }

        private async Task SetDrdlPaymentMethod()
        {
            List<PaymentMethodInfo> paymentMethods = await new PaymentMethodBLL(userBLL)
                .GetPaymentMethodsAsync();
            foreach (PaymentMethodInfo paymentMethod in paymentMethods)
            {
                drdlPaymentMethod.Items.Add(new ListItem(paymentMethod.name, paymentMethod.ID.ToString()));
            }
        }

        private bool CheckLoggedIn()
        {
            return (Session["userSession"] != null);
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvUsername,
                "txtUsername",
                "Không được trống, chỉ chứa a-z, 0-9, _ và -",
                true,
                null,
                customValidation.ValidateUsername
            );

            customValidation.Init(
                cvEmail, 
                "txtEmail", 
                "Không được để trống và phải hợp lệ", 
                true, 
                null, 
                customValidation.ValidateEmail
            );

            customValidation.Init(
                cvPhoneNumber, 
                "txtPhoneNumber", 
                "Số điện thoại không hợp lệ", 
                false,
                null, 
                customValidation.ValidatePhoneNumber
            );

            customValidation.Init(
                cvPassword, 
                "txtPassword", 
                "Tối thiểu 6 ký tự, tối đa 20 ký tự", 
                true, 
                null, 
                customValidation.ValidatePassword
            );

            cmpRePassword.ControlToValidate = "txtPassword";
            cmpRePassword.ControlToCompare = "txtRePassword";
            cmpRePassword.ErrorMessage = "Không khớp với mật khẩu mà bạn đã nhập";

            customValidation.Init(
                cvCardNumber, 
                "txtCardNumber", 
                "Số thẻ không hợp lệ", 
                false, 
                null, 
                customValidation.ValidateCardNumber
            );

            customValidation.Init(
                cvCvv, 
                "txtCvv", 
                "Số CVV không hợp lệ",
                false, 
                null,
                customValidation.ValidateCVV
            );

            customValidation.Init(
                cvAccountName, 
                "txtAccountName", 
                "Tên chủ tài khoản không hợp lệ", 
                false, 
                null, 
                customValidation.ValidateAccountName
            );

            customValidation.Init(
                cvExpirationDate, 
                "txtExpirationDate", 
                "Ngày hết hạn không hợp lệ",
                false, 
                null, 
                customValidation.ValidateExpirationDate
            );
        }

        private void ValidateData()
        {
            cvUsername.Validate();
            cvEmail.Validate();
            cvPhoneNumber.Validate();
            cvPassword.Validate();
            cmpRePassword.Validate();
            cvCardNumber.Validate();
            cvCvv.Validate();
            cvAccountName.Validate();
            cvExpirationDate.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return (
                cvUsername.IsValid && cvEmail.IsValid && cvPhoneNumber.IsValid && cvPassword.IsValid
                && cmpRePassword.IsValid && cvCardNumber.IsValid && cvCvv.IsValid && cvAccountName.IsValid
                && cvExpirationDate.IsValid
            );
        }

        private UserCreation GetUserRegister()
        {
            return new UserCreation
            {
                userName = Request.Form["txtUsername"],
                email = Request.Form["txtEmail"],
                phoneNumber = Request.Form["txtPhoneNumber"],
                password = Request.Form["txtPassword"],
                PaymentInfo = GetPaymentInfo()
            };
        }

        private PaymentInfoCreation GetPaymentInfo()
        {
            string cardNumber = Request.Form["txtCardNumber"];
            string cvv = Request.Form["txtCvv"];
            string accountName = Request.Form["txtAccountName"];
            string expirationDate = Request.Form["txtExpirationDate"];
            string paymentMethod = Request.Form["drdlPaymentMethod"];
            if (
               (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(accountName)
               || string.IsNullOrEmpty(expirationDate) || string.IsNullOrEmpty(paymentMethod)) == false
            )
            {
                return new PaymentInfoCreation
                {
                    cardNumber = cardNumber,
                    cvv = cvv,
                    owner = accountName,
                    expirationDate = expirationDate,
                    paymentMethodId = int.Parse(paymentMethod)
                };
            }
            return null;
        }

        private async Task RegisterAccount()
        {
            if (IsValidData())
            {
                UserCreation userCreation = GetUserRegister();
                UserDao.RegisterState registerState = await userBLL.RegisterAsync(userCreation);

                if (registerState == UserDao.RegisterState.Failed || registerState == UserDao.RegisterState.AlreadyExist)
                {
                    if (registerState == UserDao.RegisterState.Failed)
                        stateDetail = "Đăng ký tài khoản thất bại";
                    else
                        stateDetail = "Đã tồn tại tài khoản có thông tin này";

                    enableShowResult = true;
                }
                else
                {
                    ConfirmCode confirmCode = new ConfirmCode();
                    Session["confirmCode"] = confirmCode.Send(userCreation.email);
                    string confirmToken = confirmCode.CreateToken();
                    Session["confirmToken"] = confirmToken;

                    UserInfo userInfo = await userBLL.GetUserByUserNameAsync(userCreation.userName);

                    if (registerState == UserDao.RegisterState.Success)
                        Response.RedirectToRoute("Account_Confirm", new
                        {
                            userId = userInfo.ID,
                            confirmToken = confirmToken,
                            type = "register"
                        });
                    else
                        Response.RedirectToRoute("Account_Confirm", new
                        {
                            userId =userInfo.ID,
                            confirmToken = confirmToken,
                            type = "register_no-payment-info"
                        });
                }
            }
        }
    }
}