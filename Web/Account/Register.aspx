<%@ Page Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Account/Layouts/AccountLayout.Master" CodeBehind="Register.aspx.cs" Inherits="Web.Account.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #f8f9fa;
        }

        .login-container {
            max-width: 800px;
            margin: 100px auto;
            padding: 30px;
            border-radius: 12px;
            background-color: #ffffff;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="login-container">
        <h3 class="text-center mb-4">Đăng ký tài khoản</h3>
        <div class="mb-3">
            <label for="txtUsername" class="form-label">Tên đăng nhập</label>
            <asp:TextBox ID="txtUsername" CssClass="form-control" Text="" Placeholder="Nhập vào tên người dùng" TextMode="SingleLine" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cvUsername" CssClass="text-danger" runat="server"></asp:CustomValidator>
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="txtEmail" class="form-label">Địa chỉ Email</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" Text="" Placeholder="Nhập vào địa chỉ Email" TextMode="Email" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvEmail" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
            <div class="col-md-6 mb-3">
                <label for="txtPhoneNumber" class="form-label">Số điện thoại</label>
                <asp:TextBox ID="txtPhoneNumber" CssClass="form-control" Text="" Placeholder="Nhập vào số điện thoại" TextMode="SingleLine" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvPhoneNumber" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="txtPassword" class="form-label">Mật khẩu</label>
                <asp:TextBox ID="txtPassword" CssClass="form-control" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvPassword" CssClass="text-danger" runat="server"></asp:CustomValidator>
            </div>
            <div class="col-md-6 mb-3">
                <label for="txtRePassword" class="form-label">Xác nhận mật khẩu</label>
                <asp:TextBox ID="txtRePassword" CssClass="form-control" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cmpRePassword" runat="server"></asp:CompareValidator>
            </div>
        </div>
        <div class="d-grid">
            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" Text="Đăng nhập" />
        </div>
        <div class="mt-3">
            <asp:HyperLink ID="hylnkResetPassword" runat="server" CssClass="text-decoration-none">Bạn quên mật khẩu?</asp:HyperLink>
            |<asp:HyperLink ID="hylnkLogin" CssClass="text-decoration-none" runat="server">Đăng nhập?</asp:HyperLink>
        </div>
    </div>
</asp:Content>

<%--<!DOCTYPE html>

<body>
    <form id="frmRegister" method="post" runat="server">
        <div class="register-form">
            <div class="register-form-title">
                <h3>Đăng ký tài khoản</h3>
                <p>Hãy đăng ký để thưởng thức những tác phẩm điện ảnh hay, chất lượng nhất</p>
                <hr />
            </div>
            <div class="register-form-data">
                <div class="register-form-group register-form-group-left">
                    <h4>Thông tin cơ bản</h4>
                    <p>Tên người dùng</p>
                    <asp:TextBox ID="txtUsername" Placeholder="Nhập vào tên người dùng" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvUsername" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Địa chỉ Email</p>
                    <asp:TextBox ID="txtEmail" Placeholder="Nhập vào địa chỉ Email" Text="" TextMode="Email" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvEmail" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Số điện thoại</p>
                    <asp:TextBox ID="txtPhoneNumber" Placeholder="Nhập vào số điện thoại" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvPhoneNumber" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Mật khẩu</p>
                    <asp:TextBox ID="txtPassword" Placeholder="Nhập vào mật khẩu" Text="" TextMode="Password" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvPassword" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Xác nhận mật khẩu</p>
                    <asp:TextBox ID="txtRePassword" Placeholder="Nhập vào xác nhận mật khẩu" Text="" TextMode="Password" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CompareValidator ID="cmpRePassword" runat="server"></asp:CompareValidator>
                    </div>
                </div>
                <div class="register-form-group register-form-group-right">
                    <h4>Thông tin thanh toán</h4>
                    <p>Số thẻ</p>
                    <asp:TextBox ID="txtCardNumber" Placeholder="Nhập vào số thẻ" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvCardNumber" runat="server"></asp:CustomValidator>
                    </div>
                    <p>CVV</p>
                    <asp:TextBox ID="txtCvv" Placeholder="Nhập vào số CVV" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvCvv" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Tên chủ tài khoản</p>
                    <asp:TextBox ID="txtAccountName" Placeholder="Nhập vào tên tài khoản" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvAccountName" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Ngày hết hạn</p>
                    <asp:TextBox ID="txtExpirationDate" Placeholder="Nhập vào ngày hết hạn" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvExpirationDate" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Phương thức thanh toán</p>
                    <asp:DropDownList ID="drdlPaymentMethod" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="register-form-submit">
                <asp:Button ID="btnSubmit" CssClass="button button-red button-register" runat="server" Text="Đăng ký" />
            </div>
        </div>
    </form>
    <% if (enableShowResult)
        { %>
    <script type="text/javascript">
        var stateDetail = "<% = stateDetail %>";
        setTimeout(function () {
            alert(stateDetail);
        }, 1500);
    </script>
    <%} %>
</body>
</html>--%>
