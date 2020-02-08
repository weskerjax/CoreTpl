<%@ Page Language="C#" EnableSessionState="False" %>
<%@ Import namespace="Autofac" %>
<%@ Import namespace="Autofac.Integration.Wcf" %>
<%@ Import namespace="CoreTpl.Service" %>
<%@ Import namespace="System.Web.Mvc" %>

<% 
    IDependencyResolver resolver = DependencyResolver.Current;

    bool isRun = true;

    switch (Request["a"])
    {
        default: isRun = false; break;

        case "CassetteRebuild": /* 重建 Cassette 綑綁 */
            Bundles.RebuildCache();
            break;
    }
%>
<%=(isRun ? "Rum Complete." : "Action Not Found.")%>
