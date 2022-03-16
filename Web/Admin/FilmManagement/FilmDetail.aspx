<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="FilmDetail.aspx.cs" Inherits="Web.Admin.FilmManagement.FilmDetail" %>

<%@ Import Namespace="Data.DTO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chi tiết phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <h5 class="mt-2">Chi tiết phim</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết phim: <% = filmInfo.name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của phim</th>
                            <td><% = filmInfo.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của phim</th>
                            <td><% = filmInfo.name %></td>
                        </tr>
                        <tr>
                            <th>Quốc gia</th>
                            <td><% = filmInfo.Country.name %></td>
                        </tr>
                        <tr>
                            <th>Ngôn ngữ</th>
                            <td><% = filmInfo.Language.name %></td>
                        </tr>
                        <tr>
                            <th>Năm phát hành</th>
                            <td><% = filmInfo.releaseDate %></td>
                        </tr>
                        <tr>
                            <th>Thể loại</th>
                            <td>
                                <ul>
                                    <% foreach (CategoryInfo categoryInfo in filmInfo.Categories)
                                        {%>
                                    <li><% = categoryInfo.name %></li>
                                    <% } %>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>Thẻ tag</th>
                            <td>
                                <ul>
                                    <% foreach (TagInfo tagInfo in filmInfo.Tags)
                                        {%>
                                    <li><% = tagInfo.name %></li>
                                    <% } %>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>Công ty sản xuất</th>
                            <td><% = filmInfo.productionCompany %></td>
                        </tr>
                        <tr>
                            <th>Đạo diễn</th>
                            <td>
                                <ul>
                                    <% foreach (DirectorInfo directorInfo in filmInfo.Directors)
                                        {%>
                                    <li><% = directorInfo.name %></li>
                                    <% } %>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>Diễn viên</th>
                            <td>
                                <ul>
                                    <% foreach (CastInfo castInfo in filmInfo.Casts)
                                        {%>
                                    <li><% = castInfo.name %></li>
                                    <% } %>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>Mô tả của phim</th>
                            <td><% = filmInfo.description %></td>
                        </tr>
                        <tr>
                            <th>Ảnh</th>
                            <td>
                                <img src="<% = filmInfo.thumbnail %>" width="150" height="240" /></td>
                        </tr>
                        <tr>
                            <th>Lượt thích</th>
                            <td><% = filmInfo.upvote %></td>
                        </tr>
                        <tr>
                            <th>Lượt không thích</th>
                            <td><% = filmInfo.downvote %></td>
                        </tr>
                        <tr>
                            <th>Lượt xem</th>
                            <td><% = filmInfo.views %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của phim</th>
                            <td><% = filmInfo.createAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của phim</th>
                            <td><% = filmInfo.updateAt %></td>
                        </tr>
                        <tr>
                            <th>Công cụ</th>
                            <td>
                                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Category" CssClass="button button-green" runat="server">Thêm/xóa thể loại</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Tag" CssClass="button button-green" runat="server">Thêm/xóa thẻ tag</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Director" CssClass="button button-green" runat="server">Thêm/xóa đạo diễn</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Cast" CssClass="button button-green" runat="server">Thêm/xóa diễn viên</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Image" CssClass="button button-green" runat="server">Thêm/xóa hình ảnh</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Source" CssClass="button button-green" runat="server">Thêm/xóa nguồn video</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
