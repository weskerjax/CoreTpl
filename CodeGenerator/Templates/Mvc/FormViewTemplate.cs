﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本: 16.0.0.0
//  
//     變更這個檔案可能會導致不正確的行為，而且如果已重新產生
//     程式碼，則會遺失變更。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CodeGenerator.Templates.Mvc
{
    using CodeGenerator.Models;
    using System.Linq;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class FormViewTemplate : CodeGenerator.Templates.TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\n");
            
            #line 5 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
 
	FilePath = TableMeta.NameSpace + ".WebApp/Views/" + TableMeta.Name + "/Form.cshtml";

            
            #line default
            #line hidden
            this.Write("@model ");
            
            #line 8 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel\r\n\r\n\r\n@{\r\n\tbool isCreate = \"Create\".EqualsIgnoreCase(ViewContext.RouteDa" +
                    "ta.GetRequiredString(\"action\"));\r\n\r\n\tViewBag.Title = isCreate ? \"新增");
            
            #line 14 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("\" : \"編輯");
            
            #line 14 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write(@""";
}

@section Styles {
<style type=""text/css""></style>
}


@section Scripts {
<script type=""text/javascript"">
jQuery(function ($) {
});
</script>
}


@using (Html.BeginForm(null, null, FormMethod.Post, new
{
	action = """",
	@class = ""form-horizontal form-sm"",
	ext_one_submit = """",
	ext_exit_alert = ""資料尚未儲存，確定要離開？"",
}))
{
	@Html.HiddenFor(m => m.");
            
            #line 38 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Name));
            
            #line default
            #line hidden
            this.Write(@")

	<div class=""page-header"" ext-scroll-follow="""">
		<div class=""pull-right hidden-print"">
			<a class=""btn btn-default btn-sm"" href=""List""><i class=""fa fa-reply fa-lg""></i> 返回</a>
			<button type=""submit"" class=""btn btn-primary btn-sm""><i class=""fa fa-save fa-lg""></i> 儲存</button>
			@if (!isCreate @*&& User.AnyAct(ACT.");
            
            #line 44 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(@"Delete)*@ )
			{
				<button class=""btn btn-danger btn-sm"" type=""submit"" name=""action"" value=""delete""><i class=""fa fa-trash-o fa-lg""></i> 刪除</button>
			}
		</div>

		<h1>@ViewBag.Title</h1>
	</div>

	
	<div class=""col-sm-6"">
	@{ var labelCol = ""col-sm-4""; var wrapCol = ""col-sm-8""; }

");
            
            #line 57 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
 foreach(var col in ActiveColumns.Where(x => !x.IsPrimaryKey)) { 
            
            #line default
            #line hidden
            this.Write("\r\n\t\t<div class=\"form-group\">\r\n\t\t\t@Html.BsLabelFor(m => m.");
            
            #line 60 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(", new { @class = labelCol + \"");
            
            #line 60 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((col.IsNullable ? "" : " required")));
            
            #line default
            #line hidden
            this.Write("\" })\r\n\t\t\t<div class=\"@wrapCol\">\r\n\t\t\t\t@Html.");
            
            #line 62 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.EditBox()));
            
            #line default
            #line hidden
            this.Write("(m => m.");
            
            #line 62 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t\t\t<span class=\"form-tip\">@Html.ValidationMessageFor(m => m.");
            
            #line 63 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(")</span>\r\n\t\t\t</div>\r\n\t\t</div>\r\n");
            
            #line 66 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\FormViewTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t</div>\r\n}\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
