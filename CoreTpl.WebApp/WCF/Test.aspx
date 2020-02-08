<%@ Page Language="C#" EnableSessionState="False" %>
<%@ Import namespace="System.Web.Mvc" %>
<%@ Import namespace="Orion.API.Extensions" %>
<%@ Import namespace="CoreTpl.Service" %>
<%@ Import namespace="CoreTpl.Domain.Enums" %>

<%
    IDependencyResolver resolver = DependencyResolver.Current;

    var _ctrlCommandSvc = resolver.GetService<ICtrlCommandService>();
    _ctrlCommandSvc.GetTodoList();

    //var purchaseCodes = new[] { "18004742", "18004814", "18004814", "18005086", "18005285", "18005285" };
    //var f = _dc.F47072.Where(x => purchaseCodes.Contains( x.SZPNID)).ToList();

    Response.Write("End");

%>

<hr/>
Rum Complete.