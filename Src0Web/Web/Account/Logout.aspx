<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Web.Account.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng xuất</title>
    <link rel="icon" type="image/png" href="<% = ResolveUrl("~/account_assets/images/favicon.png") %>" />
    <link rel="stylesheet" href="<% = ResolveUrl("~/account_assets/css/logout.css") %>" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-panel">
            <div class="logout-panel-header">
                <h3>ĐĂNG XUẤT KHỎI TÀI KHOẢN</h3>
                <hr />
            </div>
            <div class="logout-panel-body">
                <p>Quay về
                    <asp:HyperLink ID="hyplnkHome" runat="server">trang chủ</asp:HyperLink></p>
                <asp:Button ID="btnLogout" CssClass="button button-red button-logout" runat="server" Text="Đăng xuất" OnClick="btnLogout_Click" />
            </div>
        </div>
    </form>
</body>
</html>
