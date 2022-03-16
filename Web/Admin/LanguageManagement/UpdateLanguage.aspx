<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="UpdateLanguage.aspx.cs" Inherits="Web.Admin.LanguageManagement.UpdateLanguage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Cập nhật ngôn ngữ - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái cập nhật ngôn ngữ</h5>
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
    <h5 class="mt-2">Cập nhật ngôn ngữ</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để cập nhật ngôn ngữ</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdLanguageId" runat="server" />
                            <asp:Label ID="lbLanguageName" runat="server" Text="Tên ngôn ngữ" AssociatedControlID="txtLanguageName"></asp:Label>
                            <asp:TextBox ID="txtLanguageName" placeholder="Nhập vào tên ngôn ngữ" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvLanguageName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbLanguageDescription" runat="server" Text="Mô tả ngôn ngữ" AssociatedControlID="txtLanguageDescription"></asp:Label>
                            <asp:TextBox ID="txtLanguageDescription" placeholder="Nhập vào mô tả ngôn ngữ" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
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
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
