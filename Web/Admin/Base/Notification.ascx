<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListSection.ascx.cs" Inherits="Web.Admin.Base.Notification" %>
<%@ Import Namespace="Common" %>

<% if (enableShowResult)
{ %>
<h5 class="mt-2">Trạng thái thực thi</h5>
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

