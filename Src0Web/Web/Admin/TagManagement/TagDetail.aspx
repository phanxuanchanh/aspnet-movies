<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="TagDetail.aspx.cs" Inherits="Web.Admin.TagManagement.TagDetail" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết thẻ tag - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <uc:NotifControl ID="notifControl" runat="server" />

    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết thẻ tag</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết thẻ tag: <% = tag.Name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của thẻ tag</th>
                            <td><% = tag.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của thẻ tag</th>
                            <td><% = tag.Name %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của thẻ tag</th>
                            <td><% = tag.Description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của thẻ tag</th>
                            <td><% = tag.CreatedAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của thẻ tag</th>
                            <td><% = tag.UpdatedAt %></td>
                        </tr>
                        <tr>
                            <th>Công cụ</th>
                            <td>
                                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                                <asp:Button ID="btnDelete" CssClass="button button-red" runat="server" Text="Xóa" OnClick="btnDelete_Click" />
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
