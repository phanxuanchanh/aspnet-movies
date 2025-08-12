<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="CategoryList.aspx.cs" Inherits="Web.User.CategoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách thể loại</title>
    <meta charset="UTF-8">
    <meta name="description" content="trang danh sách thể loại">
    <meta name="keywords" content="danh sách thể loại phim, thể loại phim">
    <meta name="author" content="">
    <link rel="profile" href="#">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="hero common-hero">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="hero-ct">
                        <h1>Danh sách thể loại</h1>
                        <%--<ul class="breadcumb">
                            <li class="active"><a href="#">Home</a></li>
                            <li><span class="ion-ios-arrow-right"></span>movie listing</li>
                        </ul>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="page-single" style="padding-top: 28px; padding-bottom: 28px;">
        <div class="container">
            <div class="row ipad-width">
                <asp:Repeater ID="rptCategories" runat="server">
                    <ItemTemplate>
                        <div class="col-sm-6 col-md-4 col-lg-3">
                            <div class="category-card" onclick="location.href='<%# Eval("Url") %>'">
                                <div class="category-card-thumb" aria-hidden="true">
                                    <img src="<% = ResolveUrl("~/user_assets/images/uploads/film-icon.png") %>"></img>
                                </div>
                                <div class="category-card-body">
                                    <div class="category-card-title"><%# Eval("Name") %></div>
                                    <div class="category-card-sub"><%# Eval("Description") %></div>
                                </div>
                                <div class="category-card-badge">120</div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
