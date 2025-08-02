<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditFilm.aspx.cs" Inherits="Web.Admin.FilmManagement.EditFilm" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="Data.DTO" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <div class="d-flex gap-2">
                <% if (isCreateAction)
                    { %>
                <span>Tạo mới phim</span>
                <% }
                    else
                    { %>
                <span>Chỉnh sửa phim <strong>{ <%= txtFilmName.Text %> | <%= hdFilmId.Value %> } </strong></span>
                <% } %>
            </div>

            <div class="d-flex gap-2">
                <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-primary" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>

        </div>
        <div class="card-body">
            <asp:HiddenField ID="hdFilmId" runat="server" />
            <div class="mb-3">
                <asp:Label ID="lbFilmName" runat="server" Text="Tên phim" AssociatedControlID="txtFilmName"></asp:Label>
                <asp:TextBox ID="txtFilmName" CssClass="form-control" placeholder="Nhập vào tên phim" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvFilmName" CssClass="text-red" runat="server"></asp:CustomValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lbFilmDescription" runat="server" Text="Mô tả phim" AssociatedControlID="txtFilmDescription"></asp:Label>
                <asp:TextBox ID="txtFilmDescription" placeholder="Nhập vào mô tả phim" CssClass="form-control text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                    <asp:Label ID="lbProductionCompany" runat="server" Text="Tên công ty sản xuất" AssociatedControlID="txtProductionCompany"></asp:Label>
                    <asp:TextBox ID="txtProductionCompany" CssClass="form-control" placeholder="Nhập vào tên công ty sản xuất" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="cvProductionCompany" CssClass="text-red" runat="server"></asp:CustomValidator>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                    <asp:Label ID="lbReleaseDate" runat="server" Text="Năm phát hành" AssociatedControlID="txtReleaseDate"></asp:Label>
                    <asp:TextBox ID="txtReleaseDate" CssClass="form-control" placeholder="Nhập vào năm phát hành" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="cvReleaseDate" CssClass="text-red" runat="server"></asp:CustomValidator>
                </div>
            </div>
            <div class="mb-3 text-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="Tạo mới" />
            </div>
        </div>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa thể loại</strong></span>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <input list="filmList" name="film" id="filmInput" class="form-control">
                <datalist id="filmList">
                    <option value="Inception">
                    <option value="Interstellar">
                    <option value="Tenet">
                </datalist>
            </div>
            <div class="table-responsive">
                <table class="table">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <td>Tên thể loại</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td></td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa thẻ tag</strong></span>
        </div>
        <div class="card-body">
        </div>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa đạo diễn</strong></span>
        </div>
        <div class="card-body">
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
