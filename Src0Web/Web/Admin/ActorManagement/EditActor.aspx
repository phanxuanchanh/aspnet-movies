<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditActor.aspx.cs" Inherits="Web.Admin.ActorManagement.EditActor" %>

<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới/sửa diễn viên - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <div class="d-flex gap-2">
                <% if (isCreateAction)
                    { %>
                <span>Tạo mới diễn viên</span>
                <% }
                    else
                    { %>
                <span>Chỉnh sửa diễn viên <strong>{ <%= txtActorName.Text %> | <%= hdActorId.Value %> } </strong></span>
                <% } %>
            </div>

            <div class="d-flex gap-2">
                <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-primary" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>

        </div>
        <div class="card-body">
            <asp:HiddenField ID="hdActorId" runat="server" />
            <div class="mb-3">
                <asp:Label ID="lbActorName" runat="server" Text="Tên thể loại" AssociatedControlID="txtActorName"></asp:Label>
                <asp:TextBox ID="txtActorName" CssClass="form-control" placeholder="Nhập vào tên thể loại" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvActorName" CssClass="text-red" runat="server"></asp:CustomValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lbActorDescription" runat="server" Text="Mô tả thể loại" AssociatedControlID="txtActorDescription"></asp:Label>
                <asp:TextBox ID="txtActorDescription" placeholder="Nhập vào mô tả thể loại" CssClass="form-control text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>

            <div class="mb-3 text-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="Tạo mới" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
