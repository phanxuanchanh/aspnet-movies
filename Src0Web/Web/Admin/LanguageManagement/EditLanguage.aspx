<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditLanguage.aspx.cs" Inherits="Web.Admin.LanguageManagement.EditLanguage" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới ngôn ngữ - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
     
    <uc:NotifControl ID="notifControl" runat="server" />

    <h5 class="mt-2">Tạo mới ngôn ngữ</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 ngôn ngữ</h3>
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
                    <asp:Button ID="btnSubmit" CssClass="button-primary" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
