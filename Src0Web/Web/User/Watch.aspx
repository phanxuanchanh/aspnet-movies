<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="Watch.aspx.cs" Inherits="Web.User.Watch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><% = title_HeadTag %> - Trang xem phim</title>
    <meta charset="UTF-8">
    <meta name="description" content="<% = description_MetaTag %>">
    <meta name="keywords" content="<% = keywords_MetaTag %>">
    <meta name="author" content="">
    <link rel="profile" href="#">

    <link href="<% = ResolveUrl("~/common_assets/video-js/video-js.min.css") %>" rel="stylesheet">
    <script src="<% = ResolveUrl("~/common_assets/video-js/video.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (filmInfo != null)
        { %>
    <div class="hero common-hero">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="hero-ct">
                        <h1>Bạn đang xem: <% = film.name %></h1>
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
            <div class="col-lg-12 col-md-12 col-sm-12" style="height: 600px;">
                <video id="vid" class="video-js vjs-default-skin" controls preload="auto" data-setup="{}">
                    <source src="<% = film.Source %>" type="video/mp4">
                    Your browser does not support the video tag.
                </video>
            </div>
        </div>
    </div>
    <style type="text/css">
        #vid {
            width: 100% !important;
            height: 600px !important;
        }
    </style>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <% if (hyplnkIncreaseView != null && film != null)
        { %>
    <script type="text/javascript">
        setTimeout(function () {
            $(document).ready(function (e) {
                $.post("<% = hyplnkIncreaseView %>", {
                    filmId: "<% = filmInfo.ID %>"
                }, function (data) {
                    console.log(data);
                });
            });
        }, 30000);
    </script>
    <%} %>
</asp:Content>
