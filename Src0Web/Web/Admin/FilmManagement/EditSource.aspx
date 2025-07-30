<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditSource.aspx.cs" Inherits="Web.Admin.FilmManagement.EditSource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chỉnh sửa nguồn video cho phim - Trang quản trị</title>
    
    <link href="<% = ResolveUrl("~/common_assets/video-js/video-js.min.css") %>" rel="stylesheet">
    <script src="<% = ResolveUrl("~/common_assets/video-js/video.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái thêm/xóa nguồn video</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <%if (stateString == "Success")
                { %>
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em><% = stateDetail %></div>
            <%}
                else if (stateString == "AlreadyExists")
                { %>
            <div class="alert background-warning"><em class="fa fa-warning"></em><% = stateDetail %></div>
            <%}
                else
                { %>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em><% = stateDetail %></div>
            <%} %>
        </div>
    </div>
    <%} %>
    <h5 class="mt-2">Thêm nguồn video cho phim</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Thêm nguồn video vào phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:Label ID="lbFilm_Source" runat="server" Text="Nguồn video" AssociatedControlID="fuFilm_Source"></asp:Label>
                            <asp:FileUpload ID="fuFilm_Source" runat="server" />
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnAdd" CssClass="button-primary" runat="server" Text="Thêm" OnClick="btnAdd_Click" />
                </div>
            </div>
        </div>
    </div>

    <% if (enableShowDetail)
        {%>
    <h5 class="mt-2">Nguồn video của phim</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Danh sách nguồn video của phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tbody>
                            <tr>
                                <th>Nguồn video</th>
                                <td>
                                    <video id="vid" class="video-js vjs-default-skin" controls preload="auto" width="640" height="264" data-setup="{}">
                                        <source src="<% = film_Source %>" type="video/mp4">
                                    </video>
                                </td>
                            </tr>
                            <tr>
                                <th>Xóa nguồn video</th>
                                <td>
                                    <asp:Button ID="btnDelete" CssClass="button-red" runat="server" Text="Xóa" OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <% } %>

    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDetail" CssClass="button button-green" runat="server">Xem chi tiết</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Category" CssClass="button button-green" runat="server">Thêm/xóa thể loại</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Tag" CssClass="button button-green" runat="server">Thêm/xóa thẻ tag</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Cast" CssClass="button button-green" runat="server">Thêm/xóa diễn viên</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Director" CssClass="button button-green" runat="server">Thêm/xóa đạo diễn</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Image" CssClass="button button-green" runat="server">Thêm/xóa hình ảnh</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
