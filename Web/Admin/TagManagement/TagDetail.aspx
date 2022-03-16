<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="TagDetail.aspx.cs" Inherits="Web.Admin.TagManagement.TagDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết thẻ tag - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết thẻ tag</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết thẻ tag: <% = tagInfo.name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của thẻ tag</th>
                            <td><% = tagInfo.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của thẻ tag</th>
                            <td><% = tagInfo.name %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của thẻ tag</th>
                            <td><% = tagInfo.description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của thẻ tag</th>
                            <td><% = tagInfo.createAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của thẻ tag</th>
                            <td><% = tagInfo.updateAt %></td>
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
