<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="UpdateFilm.aspx.cs" Inherits="Web.Admin.FilmManagement.UpdateFilm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Cập nhật phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái cập nhật phim</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <%if (stateString == "Success")
                { %>
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em><% = stateDetail %></div>
            <% }
                else
                { %>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em><% = stateDetail %></div>
            <%} %>
        </div>
    </div>
    <%} %>
    <h5 class="mt-2">Cập nhật phim</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để cập nhật phim</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdFilmId" runat="server" />
                            <asp:Label ID="lbFilmName" runat="server" Text="Tên phim" AssociatedControlID="txtFilmName"></asp:Label>
                            <asp:TextBox ID="txtFilmName" placeholder="Nhập vào tên phim" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvFilmName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbFilmCountry" runat="server" Text="Quốc gia" AssociatedControlID="drdlFilmCountry"></asp:Label>
                            <asp:DropDownList ID="drdlFilmCountry" runat="server"></asp:DropDownList>
                            <asp:Label ID="lbProductionCompany" runat="server" Text="Tên công ty sản xuất" AssociatedControlID="txtProductionCompany"></asp:Label>
                            <asp:TextBox ID="txtProductionCompany" placeholder="Nhập vào tên công ty sản xuất" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvProductionCompany" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbFilmLanguage" runat="server" Text="Ngôn ngữ" AssociatedControlID="drdlFilmLanguage"></asp:Label>
                            <asp:DropDownList ID="drdlFilmLanguage" runat="server"></asp:DropDownList>
                            <asp:Label ID="lbReleaseDate" runat="server" Text="Năm phát hành" AssociatedControlID="txtReleaseDate"></asp:Label>
                            <asp:TextBox ID="txtReleaseDate" placeholder="Nhập vào năm phát hành" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvReleaseDate" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbFilmDescription" runat="server" Text="Mô tả phim" AssociatedControlID="txtFilmDescription"></asp:Label>
                            <asp:TextBox ID="txtFilmDescription" placeholder="Nhập vào mô tả phim" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnSubmit" CssClass="button-primary" runat="server" Text="Cập nhật" />
                </div>
            </div>
        </div>
    </div>

    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDetail" CssClass="button button-green" runat="server">Xem chi tiết</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Category" CssClass="button button-green" runat="server">Thêm/xóa thể loại</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Tag" CssClass="button button-green" runat="server">Thêm/xóa thẻ tag</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Director" CssClass="button button-green" runat="server">Thêm/xóa đạo diễn</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Cast" CssClass="button button-green" runat="server">Thêm/xóa diễn viên</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Image" CssClass="button button-green" runat="server">Thêm/xóa hình ảnh</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit_Source" CssClass="button button-green" runat="server">Thêm/xóa nguồn video</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
