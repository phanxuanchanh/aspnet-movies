<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="RoleDetail.aspx.cs" Inherits="Web.Admin.RoleManagement.RoleDetail" %>

<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết vai trò - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header">
            <span>Chi tiết vai trò: <strong><%= role.Name %></strong></span>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table">
                    <tr>
                        <th>ID</th>
                        <td><% = role.ID %></td>
                    </tr>
                    <tr>
                        <th>Tên</th>
                        <td><% = role.Name %></td>
                    </tr>
                    <tr>
                        <th>Ngày tạo</th>
                        <td><% = role.CreatedAt %></td>
                    </tr>
                    <tr>
                        <th>Ngày cập nhật</th>
                        <td><% = role.UpdatedAt %></td>
                    </tr>
                    <tr>
                        <th>Công cụ</th>
                        <td>
                            <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                            <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                            <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
