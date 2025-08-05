<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditDirector.aspx.cs" Inherits="Web.Admin.DirectorManagement.EditDirector" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới/sửa đạo diễn - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    
    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
        <div class="d-flex gap-2">
            <% if (isCreateAction)
                { %>
            <span>Tạo mới đạo diễn</span>
            <% }
                else
                { %>
            <span>Chỉnh sửa thông tin đạo diễn <strong>{ <%= txtDirectorName.Text %> | <%= hdDirectorId.Value %> } </strong></span>
            <% } %>
        </div>

        <div class="d-flex gap-2">
            <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-primary" runat="server">Quay về trang danh sách</asp:HyperLink>
        </div>

    </div>
    <div class="card-body">
        <asp:HiddenField ID="hdDirectorId" runat="server" />
        <div class="mb-3">
            <asp:Label ID="lbDirectorName" runat="server" Text="Tên thể loại" AssociatedControlID="txtDirectorName"></asp:Label>
            <asp:TextBox ID="txtDirectorName" CssClass="form-control" placeholder="Nhập vào tên thể loại" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cvDirectorName" CssClass="text-red" runat="server"></asp:CustomValidator>
        </div>
        <div class="mb-3">
            <asp:Label ID="lbDirectorDescription" runat="server" Text="Mô tả thể loại" AssociatedControlID="txtDirectorDescription"></asp:Label>
            <asp:TextBox ID="txtDirectorDescription" placeholder="Nhập vào mô tả thể loại" CssClass="form-control text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>

        <div class="mb-3 text-center">
            <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="Tạo mới" />
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
