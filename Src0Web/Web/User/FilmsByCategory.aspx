<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="FilmsByCategory.aspx.cs" Inherits="Web.User.FilmsByCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Thể loại: <% = categoryName %> -- Trang xem theo thể loại</title>
    <meta charset="UTF-8">
    <meta name="description" content="trang phim theo thể loại">
    <meta name="keywords" content="phim theo thể loại">
    <meta name="author" content="">
    <link rel="profile" href="#">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="hero common-hero">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="hero-ct">
                        <h1>Phim theo thể loại</h1>
                        <%--<ul class="breadcumb">
                            <li class="active"><a href="#">Home</a></li>
                            <li><span class="ion-ios-arrow-right"></span>movie listing</li>
                        </ul>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="page-single">
        <div class="container">
            <div class="row ipad-width">
                <% if (films != null)
                    { %>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="topbar-filter">
                        <p>Tìm thấy:  <span><% = films.Count %></span> phim với thể loại này "<% = categoryName %>"</p>
                    </div>
                    <div class="flex-wrap-movielist">
                        <% foreach (FilmDto film in films)
                            {%>
                        <div class="movie-item-style-2 movie-item-style-1">
                            <img src="<% = film.Thumbnail %>" alt="<% = film.Name %>">
                            <div class="hvr-inner">
                                <a href="<% = film.Url %>">Xem chi tiết <i class="ion-android-arrow-dropright"></i></a>
                            </div>
                            <div class="mv-item-infor">
                                <h6><a href="<% = film.Url %>"><% = film.Name %></a></h6>
                                <p class="rate"><i class="ion-android-star"></i><span><% = string.Format("{0:0.00}", film.ScoreRating) %></span> /10</p>
                            </div>
                        </div>
                        <% }%>
                    </div>
                    <%--<div class="topbar-filter">
                        <label>Phim/Trang:</label>
                        <select>
                            <option value="range">20 Movies</option>
                            <option value="saab">10 Movies</option>
                        </select>

                        <div class="pagination2">
                            <span>Page 1 of 2:</span>
                            <a class="active" href="#">1</a>
                            <a href="#">2</a>
                            <a href="#">3</a>
                            <a href="#">...</a>
                            <a href="#">78</a>
                            <a href="#">79</a>
                            <a href="#"><i class="ion-arrow-right-b"></i></a>
                        </div>
                    </div>--%>
                </div>
                <%} %>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
