<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.User.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang chủ</title>
    <meta charset="UTF-8">
    <meta name="description" content="">
    <meta name="keywords" content="xem phim online, xem phim bộ, phim chiếu rạp">
    <meta name="author" content="">
    <link rel="profile" href="#">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="slider movie-items">
        <div class="container">
            <div class="row">
                <div class="social-link">
                    <p>Theo dõi chúng tôi: </p>
                    <a href="#"><i class="ion-social-facebook"></i></a>
                    <a href="#"><i class="ion-social-twitter"></i></a>
                    <a href="#"><i class="ion-social-googleplus"></i></a>
                    <a href="#"><i class="ion-social-youtube"></i></a>
                </div>
                <div class="slick-multiItemSlider">
                    <% if (latestFilms != null)
                        {
                            foreach (FilmDto film in latestFilms)
                            { %>
                    <div class="movie-item">
                        <div class="mv-img">
                            <a href="<% = film.Url %>">
                                <img src="<%= film.Thumbnail %>" alt="<% = film.Name %>"" style="width:285px; height:437px;">
                            </a>
                        </div>
                        <div class="title-in">
                            <div class="cate">
                                <% foreach (CategoryDto categoryOfFilm in film.Categories)
                                    { %>
                                <span class="blue"><a href="#"><%= categoryOfFilm.Name %></a></span>
                                <%} %>
                            </div>
                            <h6><a href="<% = film.Url %>"><% = film.Name %></a></h6>
                            <p><i class="ion-android-star"></i><span><% = string.Format("{0:0.00}", film.ScoreRating) %></span> /10</p>
                        </div>
                    </div>
                    <% }
                        }%>
                </div>
            </div>
        </div>
    </div>
    <div class="movie-items">
        <div class="container">
            <div class="row ipad-width">
                <div class="col-md-8">
                    <% foreach (KeyValuePair<CategoryDto, List<FilmDto>> filmOfCategory in films_CategoryDict)
                        { %>
                    <div class="title-hd">
                        <h2><% = filmOfCategory.Key.Name %></h2>
                        <a href="<% = filmOfCategory.Key.Url %>" class="viewall">Xem tất cả <i class="ion-ios-arrow-right"></i></a>
                    </div>
                    <div class="tabs">
                        <ul class="tab-links">
                            <li class="active"><p>Có thể bạn nên xem</p></li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab active">
                                <div class="row">
                                    <div class="slick-multiItem">
                                        <% foreach (FilmDto film in filmOfCategory.Value)
                                            { %>
                                        <div class="slide-it">
                                            <div class="movie-item">
                                                <div class="mv-img">
                                                    <img src="<% = film.Thumbnail %>" alt="<% = film.Name %>" width="185" height="284">
                                                </div>
                                                <div class="hvr-inner">
                                                    <a href="<% = film.Url %>">Xem thêm <i class="ion-android-arrow-dropright"></i></a>
                                                </div>
                                                <div class="title-in">
                                                    <h6><a href="<% = film.Url %>"><% = film.Name %></a></h6>
                                                    <p><i class="ion-android-star"></i><span><% = string.Format("{0:0.00}", film.ScoreRating) %></span> /10</p>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                </div>
                <div class="col-md-4">
                    <div class="sidebar">
                        <div class="ads">
                            <img src="<% = ResolveUrl("~/user_assets/images/uploads/banner_img.jpg") %>" alt="" width="336" height="296">
                        </div>
                        <div class="celebrities">
                            <h4 class="sb-title">Thể loại phim</h4>

                            <% if (categories != null)
                                {
                                    foreach (CategoryDto category in categories)
                                    { %>
                            <div class="celeb-item">
                                <a href="#">
                                    <img src="<% = ResolveUrl("~/user_assets/images/uploads/film-icon.png") %>" alt="" width="70" height="70"></a>
                                <div class="celeb-author">
                                    <h6><a href="<% = category.Url %>"><% = category.Name %></a></h6>
                                    <span></span>
                                </div>
                            </div>
                            <%}
                                }%>
                            <a href="<% = hyplnkCategoryList %>" class="btn">Xem tất cả thể loại<i class="ion-ios-arrow-right"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
