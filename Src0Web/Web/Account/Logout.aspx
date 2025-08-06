<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" MasterPageFile="~/Account/Layouts/AccountLayout.Master" Inherits="Web.Account.Logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đăng xuất</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="row my-5">
        <div class="col-sm-12 col-md-12 col-lg-12 d-flex justify-content-center">
            <div class="toast show" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="toast-header">
                    <strong class="me-auto">ĐĂNG XUẤT KHỎI TÀI KHOẢN</strong>
                </div>
                <div class="toast-body text-center">
                    <p>Bạn đang muốn đăng xuất?</p>
                    <asp:HyperLink ID="hyplnkHome" CssClass="btn btn-success" runat="server">Trang chủ</asp:HyperLink>
                    <asp:Button ID="btnLogout" CssClass="btn btn-danger" runat="server" Text="Đăng xuất" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
