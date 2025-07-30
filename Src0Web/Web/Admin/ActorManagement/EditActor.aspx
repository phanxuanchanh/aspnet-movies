<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditActor.aspx.cs" Inherits="Web.Admin.ActorManagement.EditActor" %>
<%@ Import Namespace="Common" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới diễn viên - Trang quản trị</title>
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

    <h5 class="mt-2">Tạo mới diễn viên</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 diễn viên</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:HiddenField ID="hdActorId" runat="server" />
                            <asp:Label ID="lbActorName" runat="server" Text="Tên diễn viên" AssociatedControlID="txtActorName"></asp:Label>
                            <asp:TextBox ID="txtActorName" placeholder="Nhập vào tên diễn viên" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvActorName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <asp:Label ID="lbActorDescription" runat="server" Text="Mô tả diễn viên" AssociatedControlID="txtActorDescription"></asp:Label>
                            <asp:TextBox ID="txtActorDescription" placeholder="Nhập vào mô tả diễn viên" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
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
