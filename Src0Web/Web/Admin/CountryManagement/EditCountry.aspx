<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditCountry.aspx.cs" Inherits="Web.Admin.CountryManagement.EditCountry" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới quốc gia - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <uc:NotifControl ID="notifControl" runat="server" />

    <h5 class="mt-2">Tạo mới quốc gia</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 quốc gia</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdCountryId" runat="server" />
                            <asp:Label ID="lbCountryName" runat="server" Text="Tên quốc gia" AssociatedControlID="txtCountryName"></asp:Label>
                            <asp:TextBox ID="txtCountryName" placeholder="Nhập vào tên quốc gia" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvCountryName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbCountryDescription" runat="server" Text="Mô tả về quốc gia" AssociatedControlID="txtCountryDescription"></asp:Label>
                            <asp:TextBox ID="txtCountryDescription" placeholder="Nhập vào mô tả quốc gia" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
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
