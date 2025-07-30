<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pagination.ascx.cs" Inherits="Web.Admin.Base.Pagination" %>
<%@ Import Namespace="Common" %>

<div class="row">
    <div class="column column-50">
        <span class="">Tổng số trang: <%= totalPages %></span>
    </div>
    <div class="column column-50">
        <ul class="pagination">
            <% if (showPrev)
                { %>
            <li class="page-item">
                <a class="page-link" href="#has">Previous</a>
            </li>
            <% }
            else
            { %>
            <li class="page-item disabled">
                <a class="page-link">Previous</a>
            </li>
            <% } %>

            <% for (long i = startPage; i <= endPage; i++)
                { %>
            <% if (i == currentPage)
                { %>
            <li class="page-item active">
                <a class="page-link"><%= i %></a>
            </li>
            <% }
                else
                { %>
            <li class="page-item">
                <a class="page-link" href="#has"><%= i %></a>
            </li>
            <% } %>
            <% } %>

            <% if (showNext)
                { %>
            <li class="page-item">
                <a class="page-link" href="#">Next</a>
            </li>
            <% }
                else
                { %>
            <li class="page-item disabled">
                <a class="page-link">Next</a>
            </li>
            <% } %>
        </ul>
    </div>
</div>
