<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="ActorList.aspx.cs" Inherits="Web.Admin.ActorManagement.ActorList" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách diễn viên - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h5 class="mt-2"></h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Danh sách diễn viên</h3>
                </div>
                <div class="card-block">
                    <asp:GridView ID="grvActor" runat="server" BorderColor="Silver" BorderStyle="Solid" AutoGenerateColumns="False" DataKeyNames="ID" OnSelectedIndexChanged="grvActor_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" />
                            <asp:BoundField DataField="Name" HeaderText="Tên diễn viên" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="Ngày tạo" />
                            <asp:BoundField DataField="UpdatedAt" HeaderText="Ngày cập nhật" />
                            <asp:CommandField AccessibleHeaderText="Chọn" ShowSelectButton="True" SelectText="Chọn diễn viên" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                      
                    </asp:GridView>
                </div>
                <div class="row ml-2 mr-2">
                    <div class="column column-20">
                        <p>Trang bạn đang xem: <% = currentPage + 1 %></p>
                    </div>
                    <div class="column column-33 redirect-to-page">
                        <p>Chuyển tới trang: </p>
                        <asp:DropDownList ID="drdlPage" runat="server" OnSelectedIndexChanged="drdlPage_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                    </div>
                    <div class="column column-20">
                        <p>Tổng số trang: <% = pageNumber %></p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%if (enableTool)
        { %>
    <h5 class="mt-2">Công cụ chỉnh sửa diễn viên</h5>
    <a class="anchor" name="buttons"></a>
    <p class="mt-2">Bạn đang thao tác trên diễn viên: <% = toolDetail %></p>
    <div class="row grid-responsive">
        <div class="column">
            <asp:HyperLink ID="hyplnkDetail" CssClass="button button-blue" runat="server">Xem chi tiết</asp:HyperLink>
            <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
            <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
        </div>
    </div>
    <%} %>

    <h5 class="mt-2">Thêm mới diễn viên</h5>
    <a class="anchor" name="buttons"></a>
    <div class="row grid-responsive">
        <div class="column">
            <asp:HyperLink ID="hyplnkCreate" CssClass="button" runat="server">Thêm mới</asp:HyperLink>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
