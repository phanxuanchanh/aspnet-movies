<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="TagDetail.aspx.cs" Inherits="Web.Admin.TagManagement.TagDetail" %>

<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết thẻ tag - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <% if (enableShowDetail)
        { %>

    <div class="card">
        <div class="card-header">
            <span>Chi tiết thẻ tag: <strong><%= tag.Name %></strong></span>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table">
                    <tr>
                        <th>ID</th>
                        <td><% = tag.ID %></td>
                    </tr>
                    <tr>
                        <th>Tên</th>
                        <td><% = tag.Name %></td>
                    </tr>
                    <tr>
                        <th>Mô tả</th>
                        <td><% = tag.Description %></td>
                    </tr>
                    <tr>
                        <th>Ngày tạo</th>
                        <td><% = tag.CreatedAt %></td>
                    </tr>
                    <tr>
                        <th>Ngày cập nhật</th>
                        <td><% = tag.UpdatedAt %></td>
                    </tr>
                    <tr>
                        <th>Công cụ</th>
                        <td>
                            <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-secondary" runat="server">Quay về trang danh sách</asp:HyperLink>
                            <asp:HyperLink ID="hyplnkEdit" CssClass="btn btn-sm btn-success" runat="server">Chỉnh sửa</asp:HyperLink>
                            <asp:Button ID="btnDelete" CssClass="btn btn-sm btn-danger" runat="server" Text="Xóa" OnClick="btnDelete_Click" />
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </div>

    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
