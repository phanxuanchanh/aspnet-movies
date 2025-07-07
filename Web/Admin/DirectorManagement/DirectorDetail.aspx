<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="DirectorDetail.aspx.cs" Inherits="Web.Admin.DirectorManagement.DirectorDetail" %>
<%@ Import Namespace="Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết đạo diễn - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
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

    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết đạo diễn</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết đạo diễn: <% = director.Name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của đạo diễn</th>
                            <td><% = director.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của đạo diễn</th>
                            <td><% = director.Name %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của đạo diễn</th>
                            <td><% = director.Description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của đạo diễn</th>
                            <td><% = director.CreatedAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của đạo diễn</th>
                            <td><% = director.UpdatedAt %></td>
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
