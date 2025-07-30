<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Admin.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang tổng quan - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <h1>Trang tổng quan!</h1>
                <p class="text-large">Quản lý hệ thống website.</p>
                <p>Đây là trang tổng quan, để thực hiện quản trị hãy vào các trang chức năng ở bên trái</p>
                <a class="button">Lượt xem trang: <% = pageVisitor %></a>
                <a class="button" style="background-color: rgb(255, 106, 0);">Số lượng phim: <% = movieNumber %></a>
                <a class="button" style="background-color: rgb(35, 107, 42);">Số lượng thể loại: <% = categoryNumber %></a>
                <a class="button" style="background-color: rgb(40, 77, 90);">Số lượng thẻ tag: <% = tagNumber %></a>
            </div>
        </div>
    </div>

    <h5 class="mt-2">Thông tin hệ thống</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Thống kê hệ thống</h3>
                </div>
                <div class="card-block">
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
                        <tr>
                            <th>Đường dẫn FFMPEG</th>
                            <td><% = systemInfo.FfmpegPath %></td>
                        </tr>
                        <tr>
                            <th>Path</th>
                            <td>
                                <% foreach (string item in systemInfo.Path_EnvironmentVariable)
                                    { %>
                                <p class="m-0"><%= item %></p>
                                <%} %>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <p class="credit">Trang quản trị -- <a href="#">Đồ án Lập trình Web</a></p>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
