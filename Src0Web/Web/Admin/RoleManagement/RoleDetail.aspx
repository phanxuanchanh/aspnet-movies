<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="RoleDetail.aspx.cs" Inherits="Web.Admin.RoleManagement.RoleDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết vai trò - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết vai trò</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết vai trò: <% = roleInfo.name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của vai trò</th>
                            <td><% = roleInfo.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của vai trò</th>
                            <td><% = roleInfo.name %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của vai trò</th>
                            <td><% = roleInfo.createAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của vai trò</th>
                            <td><% = roleInfo.updateAt %></td>
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
    </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
