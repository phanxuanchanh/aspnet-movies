<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="Web.Admin.CategoryManagement.EditCategory" %>

<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới/sửa thể loại - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <div class="d-flex gap-2">
                <% if (isCreateAction)
                    { %>
                <span>Tạo mới thể loại</span>
                <% }
                    else
                    { %>
                <span>Chỉnh sửa thể loại <strong>{ <%= txtCategoryName.Text %> | <%= hdCategoryId.Value %> } </strong></span>
                <% } %>
            </div>

            <div class="d-flex gap-2">
                <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-primary" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>

        </div>
        <div class="card-body">
            <asp:HiddenField ID="hdCategoryId" runat="server" />
            <div class="mb-3">
                <asp:Label ID="lbCategoryName" runat="server" Text="Tên thể loại" AssociatedControlID="txtCategoryName"></asp:Label>
                <asp:TextBox ID="txtCategoryName" CssClass="form-control" placeholder="Nhập vào tên thể loại" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvCategoryName" CssClass="text-red" runat="server"></asp:CustomValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lbCategoryDescription" runat="server" Text="Mô tả thể loại" AssociatedControlID="txtCategoryDescription"></asp:Label>
                <asp:TextBox ID="txtCategoryDescription" placeholder="Nhập vào mô tả thể loại" CssClass="form-control text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>

            <div class="mb-3 text-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="Tạo mới" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
