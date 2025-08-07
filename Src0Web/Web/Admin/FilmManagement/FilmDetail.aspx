<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="FilmDetail.aspx.cs" Inherits="Web.Admin.FilmManagement.FilmDetail" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="Data.DTO" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header">
            <span>Chi tiết phim: <strong><%= film.Name %></strong></span>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table">
                    <tr>
                        <th>ID của phim</th>
                        <td><% = film.ID %></td>
                    </tr>
                    <tr>
                        <th>Tên của phim</th>
                        <td><% = film.Name %></td>
                    </tr>
                    <tr>
                        <th>Quốc gia</th>
                        <td><% = film.Country.Name %></td>
                    </tr>
                    <tr>
                        <th>Ngôn ngữ</th>
                        <td><% = film.Language.Name %></td>
                    </tr>
                    <tr>
                        <th>Năm phát hành</th>
                        <td><% = film.ReleaseDate %></td>
                    </tr>
                    <tr>
                        <th>Thể loại</th>
                        <td>
                            <% foreach (CategoryDto category in film.Categories)
                                {%>
                            <span class="badge bg-info"><% = category.Name %></span>
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th>Thẻ tag</th>
                        <td>
                            <% foreach (TagDto tag in film.Tags)
                                {%>
                            <span class="badge bg-dark"><% = tag.Name %></span>
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th>Công ty sản xuất</th>
                        <td><% = film.ProductionCompany %></td>
                    </tr>
                    <tr>
                        <th>Đạo diễn</th>
                        <td>
                            <% foreach (DirectorDto director in film.Directors)
                                {%>
                            <span class="badge bg-success"><% = director.Name %></span>
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th>Diễn viên</th>
                        <td>
                            <% foreach (ActorDto actor in film.Actors)
                                {%>
                            <span class="badge bg-warning"><% = actor.Name %></span>
                            <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th>Mô tả của phim</th>
                        <td><% = film.Description %></td>
                    </tr>
                    <tr>
                        <th>Ảnh</th>
                        <td>
                            <img src="<% = film.Thumbnail %>" width="150" height="240" /></td>
                    </tr>
                    <tr>
                        <th>Lượt thích</th>
                        <td><% = film.Upvote %></td>
                    </tr>
                    <tr>
                        <th>Lượt không thích</th>
                        <td><% = film.Downvote %></td>
                    </tr>
                    <tr>
                        <th>Lượt xem</th>
                        <td><% = film.Views %></td>
                    </tr>
                    <tr>
                        <th>Ngày tạo của phim</th>
                        <td><% = film.CreatedAt %></td>
                    </tr>
                    <tr>
                        <th>Ngày cập nhật của phim</th>
                        <td><% = film.UpdatedAt %></td>
                    </tr>
                    <tr>
                        <th>Công cụ</th>
                        <td>
                            <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-secondary" runat="server">Quay về trang danh sách</asp:HyperLink>
                            <asp:HyperLink ID="hyplnkEdit" CssClass="btn btn-sm btn-success" runat="server">Chỉnh sửa</asp:HyperLink>
                            <asp:Button ID="btnDelete" CssClass="btn btn-sm btn-danger" runat="server" Text="Xóa" OnClick="btnDelete_Click" />
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
