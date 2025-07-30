<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditTag.aspx.cs" Inherits="Web.Admin.FilmManagement.EditTag" %>

<%@ Import Namespace="Data.DTO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chỉnh sửa thẻ tag cho phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái thêm/xóa thẻ tag</h5>
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
    <h5 class="mt-2">Thêm thẻ tag cho phim</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Thêm thẻ tag vào phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:Label ID="lbFilmTag" runat="server" Text="Thẻ tag" AssociatedControlID="drdlFilmTag"></asp:Label>
                            <asp:DropDownList ID="drdlFilmTag" runat="server"></asp:DropDownList>
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
    <h5 class="mt-2">Thẻ tag của phim</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Danh sách thẻ tag của phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tbody>
                            <% int count = 1; %>
                            <% foreach (TagInfo categoryInfo in tagsByFilmId)
                                {%>
                            <tr>
                                <th>Thẻ tag <% = count++ %></th>
                                <td><% = categoryInfo.name %></td>
                            </tr>
                            <% } %>
                            <tr>
                                <th>Xóa thẻ tag</th>
                                <td>
                                    <asp:Button ID="btnDelete" CssClass="button-red" runat="server" Text="Xóa tất cả" OnClick="btnDelete_Click" />
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
                <asp:HyperLink ID="hyplnkEdit_Director" CssClass="button button-green" runat="server">Thêm/xóa đạo diễn</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Cast" CssClass="button button-green" runat="server">Thêm/xóa diễn viên</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Image" CssClass="button button-green" runat="server">Thêm/xóa hình ảnh</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Source" CssClass="button button-green" runat="server">Thêm/xóa nguồn video</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
