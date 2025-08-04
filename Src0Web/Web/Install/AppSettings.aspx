<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppSettings.aspx.cs" Inherits="Web.Install.AppSettings" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Thiết lập hệ thống</title>
    <link rel="stylesheet" href='http://fonts.googleapis.com/css?family=Dosis:400,700,500|Nunito:300,400,600' />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="format-detection" content="telephone-no">
    <link rel="stylesheet" href="<%= ResolveUrl("~/common_assets/bootstrap/bootstrap.min.css") %>">
</head>
<body>
    <form id="frmAppSettings" runat="server">
        <div class="container">
            <div class="card mt-5">
                <div class="card-header">
                    <span class="text-success fw-bold">Cấu hình tài khoản quản trị</span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                            <label for="txtAdminUsername" class="form-label">Username</label>
                            <asp:TextBox ID="txtAdminUsername" CssClass="form-control" Text="" Placeholder="Username" TextMode="SingleLine" runat="server"></asp:TextBox>
                            <div id="adminUsernameHelp" class="form-text">Tên người dùng cho tài khoản Admin</div>
                            <asp:RequiredFieldValidator ID="rfvAdminUsername" ControlToValidate="txtAdminUsername" ErrorMessage="Username không được trống" runat="server" ForeColor="Red" />
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                            <label for="txtAdminPassword" class="form-label">Mật khẩu</label>
                            <asp:TextBox ID="txtAdminPassword" CssClass="form-control" Text="" Placeholder="Mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
                            <div id="adminPasswordHelp" class="form-text">Mật khẩu cho tài khoản Admin</div>
                            <asp:RequiredFieldValidator ID="rfvAdminPassword" ControlToValidate="txtAdminPassword" ErrorMessage="Mật khẩu không được trống" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12 mb-3">
                            <label for="txtAdminEmail" class="form-label">Email</label>
                            <asp:TextBox ID="txtAdminEmail" CssClass="form-control" Text="" Placeholder="Email" TextMode="Email" runat="server"></asp:TextBox>
                            <div id="adminEmailHelp" class="form-text">Email cho tài khoản Admin</div>
                            <asp:RequiredFieldValidator ID="rfvAdminEmail" ControlToValidate="txtAdminEmail" ErrorMessage="Email không được trống" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card mt-3">
                <div class="card-header">
                    <span class="text-success fw-bold">Cấu hình CDN</span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12 mb-3">
                            <label for="txtCDNHost" class="form-label">CDN Host</label>
                            <asp:TextBox ID="txtCDNHost" CssClass="form-control" Text="" Placeholder="Máy chủ CDN" TextMode="SingleLine" runat="server"></asp:TextBox>
                            <div id="cdnHostHelp" class="form-text">Hostname của máy chủ CDN</div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                            <label for="txtCDNClientId" class="form-label">Mã ClientId</label>
                            <asp:TextBox ID="txtCDNClientId" CssClass="form-control" Text="" Placeholder="Mã client id" TextMode="Password" runat="server"></asp:TextBox>
                            <div id="cdnClientIdHelp" class="form-text">Client ID để kết nối API</div>
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                            <label for="txtCDNSecretKey" class="form-label">Mã ClientId</label>
                            <asp:TextBox ID="txtCDNSecretKey" CssClass="form-control" Text="" Placeholder="Mã client id" TextMode="Password" runat="server"></asp:TextBox>
                            <div id="cdnSecretKeyHelp" class="form-text">Mã bí mật để kết nối API</div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card mt-3 mb-3">
                <div class="card-body text-center">
                    <asp:Button ID="btnSubmit" CssClass="btn btn-success" Text="Thực hiện" OnClick="btnSubmit_Click" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
