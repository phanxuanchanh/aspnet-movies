<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Notification.ascx.cs" Inherits="Web.Admin.Base.Notification" %>
<%@ Import Namespace="Common" %>

<% if (enableShowResult)
    { %>
<div class="my-2">
    <%if (commandResult.Status == ExecStatus.Success)
        { %>
    <div class="alert alert-success"><em class="fa fa-thumbs-up"></em><% = commandResult.Message %></div>
    <%}
        else if (commandResult.Status == ExecStatus.AlreadyExists)
        { %>
    <div class="alert alert-warning"><em class="fa fa-warning"></em><% = commandResult.Message %></div>
    <%}
        else
        { %>
    <div class="alert alert-danger"><em class="fa fa-times-circle"></em><% = commandResult.Message %></div>
    <%} %>
</div>
<%} %>