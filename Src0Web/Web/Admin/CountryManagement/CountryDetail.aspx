<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CountryDetail.aspx.cs" Inherits="Web.Admin.CountryManagement.CountryDetail" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết quốc gia - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <uc:NotifControl ID="notifControl" runat="server" />

    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết quốc gia</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết quốc gia: <% = country.Name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của quốc gia</th>
                            <td><% = country.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của quốc gia</th>
                            <td><% = country.Name %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của quốc gia</th>
                            <td><% = country.Description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của quốc gia</th>
                            <td><% = country.CreatedAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của quốc gia</th>
                            <td><% = country.UpdatedAt %></td>
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
