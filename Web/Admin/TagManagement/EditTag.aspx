<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditTag.aspx.cs" Inherits="Web.Admin.TagManagement.EditTag" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới thẻ tag - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
     
    <uc:NotifControl ID="notifControl" runat="server" />

    <h5 class="mt-2">Tạo mới thẻ tag</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 thẻ tag</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdTagId" runat="server" />
                            <asp:Label ID="lbTagName" runat="server" Text="Tên thẻ tag" AssociatedControlID="txtTagName"></asp:Label>
                            <asp:TextBox ID="txtTagName" placeholder="Nhập vào tên thẻ tag" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvTagName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbTagDescription" runat="server" Text="Mô tả thẻ tag" AssociatedControlID="txtTagDescription"></asp:Label>
                            <asp:TextBox ID="txtTagDescription" placeholder="Nhập vào mô tả thẻ tag" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
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
