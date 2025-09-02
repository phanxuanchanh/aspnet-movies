<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditFilm.aspx.cs" Inherits="Web.Admin.FilmManagement.EditFilm" %>

<%@ Register TagPrefix="uc" TagName="NotifControl" Src="~/Admin/Base/Notification.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Tạo mới phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <uc:NotifControl ID="notifControl" runat="server" />

    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <div class="d-flex gap-2">
                <% if (isCreateAction)
                    { %>
                <span>Tạo mới phim</span>
                <% }
                    else
                    { %>
                <span>Chỉnh sửa phim <strong>{ <%= txtFilmName.Text %> | <%= hdFilmId.Value %> } </strong></span>
                <% } %>
            </div>

            <div class="d-flex gap-2">
                <asp:HyperLink ID="hyplnkList" CssClass="btn btn-sm btn-primary" runat="server">Quay về trang danh sách</asp:HyperLink>
            </div>

        </div>
        <div class="card-body">
            <asp:HiddenField ID="hdFilmId" runat="server" />
            <div class="mb-3">
                <asp:Label ID="lbFilmName" runat="server" Text="Tên phim" AssociatedControlID="txtFilmName"></asp:Label>
                <asp:TextBox ID="txtFilmName" CssClass="form-control" placeholder="Nhập vào tên phim" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="cvFilmName" CssClass="text-red" runat="server"></asp:CustomValidator>
            </div>
            <div class="mb-3">
                <asp:Label ID="lbFilmDescription" runat="server" Text="Mô tả phim" AssociatedControlID="txtFilmDescription"></asp:Label>
                <asp:TextBox ID="txtFilmDescription" placeholder="Nhập vào mô tả phim" CssClass="form-control text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                    <asp:Label ID="lbProductionCompany" runat="server" Text="Tên công ty sản xuất" AssociatedControlID="txtProductionCompany"></asp:Label>
                    <asp:TextBox ID="txtProductionCompany" CssClass="form-control" placeholder="Nhập vào tên công ty sản xuất" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="cvProductionCompany" CssClass="text-red" runat="server"></asp:CustomValidator>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6 mb-3">
                    <asp:Label ID="lbReleaseDate" runat="server" Text="Năm phát hành" AssociatedControlID="txtReleaseDate"></asp:Label>
                    <asp:TextBox ID="txtReleaseDate" CssClass="form-control" placeholder="Nhập vào năm phát hành" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="cvReleaseDate" CssClass="text-red" runat="server"></asp:CustomValidator>
                </div>
            </div>

            <div class="mb-3 text-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-success" OnClick="btnSubmit_Click" runat="server" Text="Tạo mới" />
            </div>
        </div>
    </div>

    <% if (!isCreateAction)
        { %>
    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa thể loại</strong></span>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <input name="film" id="searchCategory" class="form-control">
                <ul id="categorySearchList" style="display: none; border: 1px solid #ccc; position: absolute; background: white; list-style: none; padding: 0; margin: 0; max-height: 150px; overflow-y: auto; width: 200px;">
                    <li style="padding: 4px; cursor: pointer">a</li>
                </ul>
            </div>
            <div class="table-responsive">
                <table class="table table-bordered table-hover">
                    <thead class="table-warning">
                        <tr>
                            <th>Tên thể loại</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptCategories" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Name") %></td>
                                    <td>
                                        <asp:LinkButton CssClass="btn btn-sm btn-danger" ID="lnkDelete" runat="server" OnCommand="lnkCategoryDelete_Command" CommandArgument='<%# Eval("ID").ToString() %>'>Xóa</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa thẻ tag</strong></span>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <input name="film" id="searchTag" class="form-control">
                <ul id="tagSearchList" style="display: none; border: 1px solid #ccc; position: absolute; background: white; list-style: none; padding: 0; margin: 0; max-height: 150px; overflow-y: auto; width: 200px;">
                    <li style="padding: 4px; cursor: pointer">a</li>
                </ul>
            </div>
            <div class="table-responsive">
                <table class="table table-bordered table-hover">
                    <thead class="table-warning">
                        <tr>
                            <th>Tên thẻ tag</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptTags" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Name") %></td>
                                    <td>
                                        <asp:LinkButton CssClass="btn btn-sm btn-danger" ID="lnkDelete" runat="server" OnCommand="lnkTagDelete_Command" CommandArgument='<%# Eval("ID").ToString() %>'>Xóa</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="card mt-3">
        <div class="card-header">
            <span><strong>Chỉnh sửa đạo diễn</strong></span>
        </div>
        <div class="card-body">
        </div>
    </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" runat="server">
    <% if (!isCreateAction)
        { %>
    <script type="text/javascript">
        const categorySearchList = document.getElementById('categorySearchList');
        let timeoutId;
        document.getElementById('searchCategory').addEventListener('input', function (e) {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(function () {
                const searchText = e.target.value;

                const formData = new FormData();
                formData.append('action', 'loadCategories');
                formData.append('categorySearchText', searchText);

                fetch('<%= GetRouteUrl("Admin_FilmDependentData", null) %>', {
                    method: 'POST',
                    body: formData
                })
                    .then(res => res.json())
                    .then(execResult => {
                        const paged = execResult.Data;

                        if (paged.Items.length > 0) {
                            categorySearchList.style.display = 'block';
                            categorySearchList.innerText = '';

                            for (let item of paged.Items) {
                                let li = document.createElement("li");
                                li.setAttribute('data-id', item.ID);
                                li.setAttribute('class', 'category-search-item');
                                li.addEventListener('click', function () {
                                    const id = li.getAttribute('data-id');
                                    __doPostBack("CategorySelected_Click", id);
                                });
                                li.innerText = item.Name;
                                categorySearchList.appendChild(li);
                            }
                        }
                    })
                    .catch(err => console.error(err));
            }, 1000);
        });
    </script>
    <% } %>
</asp:Content>
