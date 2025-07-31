<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CountryList.aspx.cs" Inherits="Web.Admin.CountryManagement.CountryList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="Data.DTO" %>
<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>
<%@ Register TagPrefix="uc" TagName="PaginationControl" Src="~/Admin/Base/Pagination.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách quốc gia - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc:NotifControl ID="notifControl" runat="server" />

    <h5 class="mt-2"></h5>
    <div class="row grid-responsive">
        <div class="col-12">
            <div class="card">
                <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                    <div class="d-flex gap-2">
                        <span>Danh sách quốc gia</span> | 
                        <asp:HyperLink ID="hyplnkCreate" CssClass="btn btn-sm btn-success" runat="server">Thêm</asp:HyperLink>
                    </div>

                    <div class="d-flex gap-2">
                        <asp:TextBox CssClass="form-control" ID="txtSearch" TextMode="SingleLine" OnTextChanged="txtSearch_TextChanged" AutoPostBack="true" runat="server" placeholder="Tìm kiếm"></asp:TextBox>
                    </div>
                </div>
                <div class="card-body">
                    <div class="my-3">
                        <label class="d-inline-block me-2">Số trang: <%= paged.TotalPages %></label>
                        | 
                    <label class="d-inline-block me-2">Số bản ghi: <%= paged.TotalItems %></label>
                        | 
                    <label class="d-inline-block">Số bản ghi/trang:</label>
                        <asp:TextBox CssClass="form-control d-inline-block w-25 me-2" ID="txtPageSize" TextMode="Number" OnTextChanged="txtPageSize_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-hover">
                            <thead class="">
                                <tr>
                                    <th scope="col" class="text-center">
                                        <input type="checkbox" id="selectAll">
                                    </th>
                                    <th scope="col">Tên quốc gia</th>
                                    <th scope="col">Ngày tạo</th>
                                    <th scope="col">Công cụ</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptCountries" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-center">
                                                <input type="checkbox" name="selected_ids[]" value="<%# Eval("ID") %>">
                                            </td>
                                            <td><%# Eval("Name") %></td>
                                            <td></td>
                                            <td>
                                                <a class="btn btn-sm btn-success" href="<%# GetRouteUrl("Admin_CountryDetail", new { id = Eval("ID") }) %>">Chi tiết</a> | 
                                                <a class="btn btn-sm btn-warning" href="<%# GetRouteUrl("Admin_EditCountry", new { id = Eval("ID"), action = "update"}) %>">Sửa</a> | 
                                                <asp:LinkButton CssClass="btn btn-sm btn-danger" ID="lnkDelete" runat="server" OnCommand="lnkDelete_Command" CommandArgument='<%# Eval("ID").ToString() %>'>Xóa</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <div class="d-flex justify-content-between align-items-center">
                        <div class="d-flex gap-2">
                            <div class="dropdown d-none d-sm-block">
                                <button class="btn btn-sm btn-outline-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                                    Hàng loạt
                                </button>
                                <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                    <li><a class="dropdown-item text-danger border-top" href="#" onclick="bulkDelete();">Xóa</a></li>
                                </ul>
                            </div>
                            <button class="btn btn-sm btn-warning">Thùng rác</button>
                        </div>
                        <div class="d-flex gap-2">
                            <uc:PaginationControl ID="pagination" runat="server"></uc:PaginationControl>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
</asp:Content>
