<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Admin.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang tổng quan - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <div class="card">
        <div class="card-body">
            <h1>Trang tổng quan!</h1>
            <p class="text-large">Quản lý hệ thống website.</p>
            <p>Đây là trang tổng quan, để thực hiện quản trị hãy vào các trang chức năng ở bên trái</p>
            <a class="p-2 rounded bg-info fw-bold text-dark text-decoration-none">Lượt xem trang: <% = pageVisitor %></a>
            <a class="p-2 rounded fw-bold text-dark text-decoration-none" style="background-color: rgb(255, 106, 0);">Số lượng phim: <% = movieNumber %></a>
            <a class="p-2 rounded fw-bold text-white text-decoration-none" style="background-color: rgb(35, 107, 42);">Số lượng thể loại: <% = categoryNumber %></a>
            <a class="p-2 rounded fw-bold text-white text-decoration-none" style="background-color: rgb(40, 77, 90);">Số lượng thẻ tag: <% = tagNumber %></a>
        </div>
    </div>

    <h5 class="mt-3">Thông tin hệ thống</h5>
    <div class="card">
        <div class="card-body">
            <table>
                <tr>
                    <th>Bộ nhớ RAM đã sử dụng</th>
                    <td><% = systemInfo.MemoryUsed %></td>
                </tr>
                <tr>
                    <th>Tổng bộ nhớ RAM của hệ thống</th>
                    <td><% = systemInfo.TotalMemory %></td>
                </tr>
                <tr>
                    <th>Hệ điều hành</th>
                    <td><% = systemInfo.OSName %></td>
                </tr>
                <tr>
                    <th>Địa chỉ IP cục bộ</th>
                    <td><% = systemInfo.IpAddress %></td>
                </tr>
                <tr>
                    <th>Địa chỉ IP công cộng</th>
                    <td><% = systemInfo.ExtenalIpAddress %></td>
                </tr>
            </table>
        </div>
    </div>

    <h5 class="mt-3">Media Server</h5>
    <div class="card">
        <div class="card-body">
            <table>
                <tr>
                    <th>Trạng thái kết nối</th>
                    <td><% = "oke" %></td>
                </tr>
                <tr>
                    <th>Hostname</th>
                    <td><% =mediaServerSetting["CdnHost"] %></td>
                </tr>
            </table>
        </div>
    </div>

    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
