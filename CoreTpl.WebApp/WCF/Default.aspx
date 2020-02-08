<%@ Page Language="C#" EnableSessionState="False" %>
<%@ Import namespace="System.Web.Routing" %>
<%@ Import namespace="System.ServiceModel.Activation" %>
<%@ Import namespace="Orion.API" %>
<%@ Import namespace="Orion.API.Extensions" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title></title>
</head>
<body>
	<ul>
		<%
			List<string> list = RouteTable.Routes.OfType<ServiceRoute>()
				.ToList(x => "~/" + x.Url.Replace("/{*pathInfo}", ""));

            list.Add("~/WCF/Execute.aspx?a=CassetteRebuild");
			list.Add("~/WCF/TableSize.aspx");

		%>
		<% foreach (var path in list) { %>
			<li><a href="<%= VirtualPathUtility.ToAbsolute(path) %>"><%= path %></a><br/>&nbsp;</li>
		<% } %>
	</ul>
</body>
</html>