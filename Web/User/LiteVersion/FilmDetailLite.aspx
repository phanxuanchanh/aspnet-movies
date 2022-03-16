<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayoutLite.Master" AutoEventWireup="true" CodeBehind="FilmDetailLite.aspx.cs" Inherits="Web.User.LiteVersion.FilmDetailLite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% if (filmInfo != null)
        {%>
    <title><% = filmInfo.name %> - Trang chi tiết - Phiên bản rút gọn</title>
    <%} %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% if (filmInfo != null)
        { %>
    <div class="main-content-title">
        <h3>Chi tiết phim: <% = filmInfo.name %></h3>
    </div>

    <div class="film-detail">
        <div class="film-image">
            <img src="<% = filmInfo.thumbnail %>" alt="<% = filmInfo.name %>" />
        </div>
        <div class="film-description">
            <h3>Mô tả phim</h3>
            <p><% = filmInfo.description %></p>
        </div>
        <div class="film-more-detail">
            <h3>Các thông tin bổ sung</h3>
            <p>Quốc gia: <% = filmInfo.Country.name %></p>
            <p>Ngôn ngữ: <% = filmInfo.Language.name %></p>
            <p>Công ty SX: <% = filmInfo.productionCompany %></p>
            <p>Ngày phát hành: <% = filmInfo.releaseDate %></p>
            <p style="color:red;">Các thông tin như đạo diễn, diễn viên, thẻ tag,... không được hiển thị trong phiên bản rút gọn, vui lòng chuyển qua phiên bản đầy đủ để xem chi tiết</p>
        </div>
        <div class="film-tool">
            <a href="<% = filmInfo.url %>">Xem phim ngay</a>
        </div>
    </div>
    <%} %>
</asp:Content>
