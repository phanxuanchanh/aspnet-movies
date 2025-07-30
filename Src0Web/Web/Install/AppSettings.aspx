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
                    <span class="text-warning fw-bold">Cấu hình SMTP</span>
                </div>
                <div class="card-body">
                </div>
            </div>

            <div class="card mt-3">
                <div class="card-header">
                    <span class="text-success fw-bold">Cấu hình lưu trữ</span>
                </div>
                <div class="card-body">
                </div>
            </div>
        </div>
    </form>
</body>
</html>
