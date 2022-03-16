<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayoutLite.Master" AutoEventWireup="true" CodeBehind="FilmsByCategoryLite.aspx.cs" Inherits="Web.User.LiteVersion.FilmsByCategoryLite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Thể loại: <% = categoryName %> -- Trang xem theo thể loại - Phiên bản rút gọn</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="main-content-title">
        <h3>Phim của thể loại: <% = categoryName %></h3>
    </div>

    <div class="film-list">
        <asp:DataList ID="dlFilm" runat="server" RepeatColumns="4" CssClass="datalist" ItemStyle-CssClass="item">
            <ItemTemplate>
                <asp:Image ID="imgItem" runat="server" CssClass="image-item" ImageUrl='<%# Eval("thumbnail") %>' Width="185px" />
                <br />
                <asp:HyperLink ID="hyplnkItem" runat="server" CssClass="hyperlink-item" NavigateUrl='<%# Eval("url") %>' Text='<%# Eval("name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
