<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="Web.Admin.CategoryManagement.CreateCategory" %>
<%@ Import Namespace="Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới thể loại - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
    { %>
    <h5 class="mt-2">Trạng thái thêm quốc gia</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <%if (commandResult.Status == ExecStatus.Success)
        { %>
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em><% = commandResult.Message %></div>
            <%}
        else if (commandResult.Status == ExecStatus.AlreadyExists)
        { %>
            <div class="alert background-warning"><em class="fa fa-warning"></em><% = commandResult.Message %></div>
            <%}
        else
        { %>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em><% = commandResult.Message %></div>
            <%} %>
        </div>
    </div>
    <%} %>

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
