<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayoutLite.Master" AutoEventWireup="true" CodeBehind="IndexLite.aspx.cs" Inherits="Web.User.LiteVersion.IndexLite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang chủ - Phiên bản rút gọn</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="main-content-title">
        <h3>Phim mới nhất</h3>
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
