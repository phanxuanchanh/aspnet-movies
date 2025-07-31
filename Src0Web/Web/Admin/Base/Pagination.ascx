<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pagination.ascx.cs" Inherits="Web.Admin.Base.Pagination" %>
<%@ Import Namespace="Common" %>

<ul class="pagination m-0">
    <% if (showPrev)
        { %>
    <li class="page-item">
        <a class="page-link" href="<%= GetRouteUrl(routeName, new { page = prevPage, pageSize = pageSize }) %>">Previous</a>
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
        <a class="page-link" href="<%= GetRouteUrl(routeName, new { page = i, pageSize = pageSize }) %>"><%= i %></a>
    </li>
    <% }
        else
        { %>
    <li class="page-item">
        <a class="page-link" href="<%= GetRouteUrl(routeName, new { page = i, pageSize = pageSize }) %>"><%= i %></a>
    </li>
    <% } %>
    <% } %>

    <% if (showNext)
        { %>
    <li class="page-item">
        <a class="page-link" href="<%= GetRouteUrl(routeName, new { page = showNext, pageSize = pageSize }) %>">Next</a>
    </li>
    <% }
        else
        { %>
    <li class="page-item disabled">
        <a class="page-link">Next</a>
    </li>
    <% } %>
</ul>


