<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="Web.Admin.CategoryManagement.EditCategory" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới thể loại - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <uc:NotifControl ID="notifControl" runat="server" />

    <h5 class="mt-2">Tạo mới thể loại</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 thể loại</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdCategoryId" runat="server" />
                            <asp:Label ID="lbCategoryName" runat="server" Text="Tên thể loại" AssociatedControlID="txtCategoryName"></asp:Label>
                            <asp:TextBox ID="txtCategoryName" placeholder="Nhập vào tên thể loại" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvCategoryName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbCategoryDescription" runat="server" Text="Mô tả thể loại" AssociatedControlID="txtCategoryDescription"></asp:Label>
                            <asp:TextBox ID="txtCategoryDescription" placeholder="Nhập vào mô tả thể loại" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnSubmit" CssClass="button-primary" runat="server" Text="Tạo mới" />
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
