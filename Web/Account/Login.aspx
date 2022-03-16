<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Account.Login" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng nhập</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" type="image/png" href="<% = ResolveUrl("~/account_assets/images/favicon.png") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/account_assets/css/login.css") %>">
    <script type="text/javascript" src="<%= ResolveUrl("~/common_assets/js/video.js") %>"></script>
    <link rel="stylesheet" href="<% = ResolveUrl("~/account_assets/fonts/fontawesome/css/all.min.css") %>">
</head>
<body>
    <form id="frmLogin" method="post" runat="server">
        <video id="backgroundVideo" class="background-video" autoplay>
            <source src="<% = ResolveUrl("~/account_assets/videos/Puples-33590.mp4") %>" type='video/mp4'>
            <p>Không thể phát video</p>
        </video>
        <div class="audio-tool">
            <div class="space">
            </div>
            <div class="audio-control" onclick="switchAudioState();">
                <i id="audioControlTitle" class="fa fa-volume-up"></i>
            </div>
        </div>
        <% if (enableShowResult)
            { %>
        <div class="login-status">
            <div class="login-status-space"></div>
            <div class="login-status-message">
                <h3><% = stateDetail %></h3>
            </div>
        </div>
        <%} %>
        <div class="video-info">
            <h3>Bạn đang xem? </h3>
            <h4>Marvel Studios' Loki | Official Trailer | Disney+</h4>
            <p class="cc">Loki’s time has come. Watch the brand-new trailer for "Loki," and start streaming the Marvel Studios Original Series June 11 on Disney+.</p>
        </div>
        <div class="notify">
            <div class="notify-title">
                <h3>Bạn không thể không xem</h3>
                <hr />
            </div>
            <div class="notify-list">
                <% for (int i = 0; i < 5; i++)
                    { %>
                <div class="notify-item">
                    <h5>Phim <%= i %></h5>
                    <p>Mô tả phim <%= i %></p>
                </div>
                <% } %>
            </div>
        </div>
        <div class="account-form login">
            <div class="account-form-title">
                <h3>Đăng nhập</h3>
                <p>Đăng nhập ngay để không bỏ lỡ những bộ phim hấp dẫn</p>
            </div>
            <div class="account-form-data">
                <asp:TextBox ID="txtUsername" Text="" Placeholder="Nhập vào tên người dùng" TextMode="SingleLine" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtPassword" Text="" Placeholder="Nhập vào mật khẩu" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="account-form-submit">
                <asp:Button ID="btnLogin" CssClass="button button-red button-login" runat="server" Text="Đăng nhập" />
            </div>
            <div class="show-error">
                <div class="show-error-item">
                    <asp:CustomValidator ID="cvUsername" runat="server"></asp:CustomValidator>
                </div>
                <div class="show-error-item">
                    <asp:CustomValidator ID="cvPassword" runat="server"></asp:CustomValidator>
                </div>
            </div>
            <div class="account-form-support">
                <span>
                    <asp:HyperLink ID="hylnkResetPassword" runat="server">Bạn quên mật khẩu?</asp:HyperLink></span>
                <span>
                    <asp:HyperLink ID="hylnkRegister" runat="server">Bạn chưa có tài khoản?</asp:HyperLink></span>
                <span>
                    <asp:HyperLink ID="hylnkFeedback" runat="server">Gửi ý kiến phản hồi</asp:HyperLink></span>
                <span>
                    <asp:HyperLink ID="hylnkContact" runat="server">Liên hệ với chúng tôi</asp:HyperLink></span>
                <span>
                    <asp:HyperLink ID="hylnkTermOfUse" runat="server">Điều khoản sử dụng dịch vụ</asp:HyperLink></span>
            </div>
        </div>
        <script src="<%= ResolveUrl("~/account_assets/js/login.js") %>"> </script>
    </form>
</body>
</html>
