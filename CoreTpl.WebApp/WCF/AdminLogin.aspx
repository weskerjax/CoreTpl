<%@ Page Language="C#" %>
<%@ Import namespace="System.Web.Mvc" %>
<%@ Import namespace="Orion.API" %>
<%@ Import namespace="Orion.Mvc.Security" %>
<%@ Import namespace="CoreTpl.Domain.Enums" %>

<% 

    List<string> actList = OrionUtils.GetEnumValues<ACT>().Select(x => x.ToString()).ToList();
    actList.Add("DevelopAdmin");

    var signInManager = DependencyResolver.Current.GetService<ISignInManager>();
    signInManager.DevelopSignIn(actList);

    //Response.Redirect("~/");
%>
