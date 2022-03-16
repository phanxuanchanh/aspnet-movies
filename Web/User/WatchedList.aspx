<%@ Page Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="WatchedList.aspx.cs" Inherits="Web.User.WatchedList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Phim bạn đã xem</title>
    <meta charset="UTF-8">
    <meta name="description" content="lịch sử xem phim">
    <meta name="keywords" content="phim đã xem, lịch sử xem phim">
    <meta name="author" content="">
    <link rel="profile" href="#">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="hero common-hero">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="hero-ct">
                        <h1>Phim bạn đã xem</h1>
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
            <div class="col-lg-12 col-md-12 col-sm-12">
                <h5 id="txtState" style="color: #ffffff;" runat="server"></h5>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12">
                <asp:DataList ID="dlWatchedList" runat="server">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Image ID="Image2" runat="server" Width="185" Height="284" ImageUrl='<%# Eval("thumbnail") %>' /></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label3" runat="server" ForeColor="White" Text='<%# Eval("name") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" ForeColor="White" Text='<%# Eval("timestamp") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="HyperLink2" NavigateUrl='<%# Eval("url") %>' ForeColor="White" runat="server">Xem chi tiết</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <% if (enableShowButton)
                { %>
            <div class="col-lg-12 col-md-12 col-sm-12" style="text-align:center;">
                <asp:Button ID="btnDelete" CssClass="btn btn-danger" runat="server" Text="Xóa lịch sử" OnClick="btnDelete_Click" />
            </div>
            <%} %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>