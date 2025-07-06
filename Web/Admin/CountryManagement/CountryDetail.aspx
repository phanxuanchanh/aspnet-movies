<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CountryDetail.aspx.cs" Inherits="Web.Admin.CountryManagement.CountryDetail" %>
<%@ Import Namespace="Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết quốc gia - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
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
                                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
                                <asp:Button ID="btnDelete" CssClass="button button-red" runat="server" Text="Xóa" OnClick="btnDelete_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%} %>

    <% if (enableShowResult)
    { %>
    <h5 class="mt-2">Trạng thái thêm quốc gia</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <%if (commandResult.Status == ExecStatus.Success)
            { %>
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em><% = commandResult.Message %></div>
            <%}
            else if (commandResult.Status == ExecStatus.AlreadyExists)
            { %>
            <div class="alert background-warning"><em class="fa fa-warning"></em><% = commandResult.Message %></div>
            <%}
            else
            { %>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em><% = commandResult.Message %></div>
            <%} %>
        </div>
    </div>
    <%} %>

    <% if(enableEditForm)
        { %>
    <h5 class="mt-2">Tạo mới quốc gia</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 quốc gia</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:Label ID="lbCountryName" runat="server" Text="Tên quốc gia" AssociatedControlID="txtCountryName"></asp:Label>
                            <asp:TextBox ID="txtCountryName" placeholder="Nhập vào tên quốc gia" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvCountryName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbCountryDescription" runat="server" Text="Mô tả về quốc gia" AssociatedControlID="txtCountryDescription"></asp:Label>
                            <asp:TextBox ID="txtCountryDescription" placeholder="Nhập vào mô tả quốc gia" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnSubmit" CssClass="button-primary" runat="server" Text="Tạo mới" />
                </div>
            </div>
        </div>
    </div>

    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <asp:HyperLink ID="HyperLink1" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
