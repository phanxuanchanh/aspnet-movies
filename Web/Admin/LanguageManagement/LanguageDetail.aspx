<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="LanguageDetail.aspx.cs" Inherits="Web.Admin.LanguageManagement.LanguageDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết ngôn ngữ - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết ngôn ngữ</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết ngôn ngữ: <% = languageInfo.name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của ngôn ngữ</th>
                            <td><% = languageInfo.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của ngôn ngữ</th>
                            <td><% = languageInfo.name %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của ngôn ngữ</th>
                            <td><% = languageInfo.description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của ngôn ngữ</th>
                            <td><% = languageInfo.createAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của ngôn ngữ</th>
                            <td><% = languageInfo.updateAt %></td>
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
