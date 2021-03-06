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
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ControllersTemplate : CodeGenerator.Templates.TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("\r\n");
            
            #line 4 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 
	string className = TableMeta.Name + "Controller";
	FilePath = TableMeta.NameSpace + ".WebApp/Controllers/" + className + ".cs";

            
            #line default
            #line hidden
            this.Write(@"using System;
using System.Net;
using JustWin.API;
using JustWin.API.Extensions;
using JustWin.API.Models;
using JustWin.Mvc.Attributes;
using JustWin.Mvc.Security;
using JustWin.Mvc.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;
using ");
            
            #line 18 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Service;\r\nusing ");
            
            #line 19 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Domain;\r\nusing ");
            
            #line 20 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Domain.Enums;\r\nusing ");
            
            #line 21 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".WebApp.Models;\r\n\r\n\r\nnamespace ");
            
            #line 24 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".WebApp.Controllers \r\n{\r\n\t/// <summary>");
            
            #line 26 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n\t//TODO [ActAuthorize(ACT.");
            
            #line 27 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("View)]\r\n\tpublic class ");
            
            #line 28 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(@" : Controller 
	{
		public IServiceContext Svc { get; set; }


		private int _userId;

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			_userId = User.Identity.GetUserId();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}




		private ");
            
            #line 47 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain toDomain(");
            
            #line 47 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel vm)\r\n\t\t{\r\n\t\t\tif(vm == null) { return null; }\r\n\r\n\t\t\tvar domain =  new ");
            
            #line 51 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain\r\n\t\t\t{\r\n");
            
            #line 53 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 foreach(var col in ActiveColumns) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 54 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(" = vm.");
            
            #line 54 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 55 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 } 
            
            #line default
            #line hidden
            
            #line 56 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 foreach(var col in WithColumns("ModifyBy", "ModifiedBy")) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 57 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(" =  _userId,\r\n");
            
            #line 58 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t};\r\n\r\n\t\t\treturn domain;\r\n\t\t}\r\n\r\n\t\t//FIXME For Edit (Get) Use Or Remove\r\n\t\tpriv" +
                    "ate ");
            
            #line 65 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel toViewModel(");
            
            #line 65 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain)\r\n\t\t{\r\n\t\t\tif (domain == null) { return null; }\r\n\r\n\t\t\treturn new ");
            
            #line 69 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel\r\n\t\t\t{\r\n");
            
            #line 71 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 foreach(var col in TableMeta.Columns) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 72 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(" = domain.");
            
            #line 72 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 73 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"			};
		}


		//private ActionResult forbidden()
		//{
		//	return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
		//}





		public ActionResult Index()
		{
			return RedirectToAction(nameof(List));
		}

			   		 


		//[SearchRemember(Ignore = new[] { ""export"" })]
		public ActionResult List(WhereParams<");
            
            #line 96 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> findParam, PageParams<");
            
            #line 96 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> pageParams, bool export = false)\r\n\t\t{\r\n\t\t\tif (export) { pageParams.PageIn" +
                    "dex = 1; pageParams.PageSize = -1; }\r\n\r\n\t\t\tPagination<");
            
            #line 100 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> domainPage = Svc.");
            
            #line 100 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(".GetPagination(findParam, pageParams);\r\n\t\t\tViewBag.Pagination = domainPage;\r\n\r\n\t\t" +
                    "\tif (!export) { return View(); }\r\n\t\t\treturn this.ExcelView($\"");
            
            #line 104 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("-{DateTime.Now:yyyyMMddHHmmss}.xls\");\r\n\t\t}\r\n\r\n\r\n\r\n\t\t[HttpGet]\r\n\t\t[UseViewPage(\"Fo" +
                    "rm\")]\r\n\t\t//TODO [ActAuthorize(ACT.");
            
            #line 111 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Create)]\r\n\t\tpublic ActionResult Create()\r\n\t\t{\r\n\t\t\tvar vm = new ");
            
            #line 114 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel\r\n\t\t\t{\r\n\t\t\t\t//TODO\r\n\t\t\t};\r\n\r\n\t\t\treturn View(vm);\r\n\t\t}\r\n\r\n\r\n\r\n\r\n\t\t[HttpPo" +
                    "st]\r\n\t\t[UseViewPage(\"Form\")]\r\n\t\t//TODO [ActAuthorize(ACT.");
            
            #line 127 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Create)]\r\n\t\tpublic ActionResult Create(");
            
            #line 128 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel vm)\r\n\t\t{\r\n\t\t\tif (!ModelState.IsValid) { return View(vm); }\r\n\r\n\t\t\t");
            
            #line 132 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain = toDomain(vm);\r\n\r\n\t\t\t");
            
            #line 134 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 134 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(" = Svc.");
            
            #line 134 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(".Save(domain);\r\n\r\n\t\t\tthis.SetStatusSuccess(\"儲存成功!!\");\r\n\t\t\treturn RedirectToAction" +
                    "(nameof(Edit), new { ");
            
            #line 137 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(" });\r\n\t\t}\r\n\r\n\t\t \r\n\r\n\r\n\t\t\r\n\t\t[HttpGet]\r\n\t\t[UseViewPage(\"Form\")]\r\n\t\t//TODO [ActAuth" +
                    "orize(ACT.");
            
            #line 146 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Edit)]\r\n\t\tpublic ActionResult Edit(");
            
            #line 147 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 147 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t{\r\n\t\t\t");
            
            #line 149 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain = Svc.");
            
            #line 149 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(".GetById(");
            
            #line 149 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n\t\t\tvar vm =  new ");
            
            #line 151 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel\r\n\t\t\t{\r\n");
            
            #line 153 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 foreach(var col in TableMeta.Columns) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t");
            
            #line 154 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(" = domain.");
            
            #line 154 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 155 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t};\r\n\r\n\t\t\treturn View(vm);\r\n\t\t}\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\t\t[HttpPost]\r\n\t\t[UseViewPage(\"Fo" +
                    "rm\")]\r\n\t\t//TODO [ActAuthorize(ACT.");
            
            #line 169 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Edit)]\r\n\t\tpublic ActionResult Edit(");
            
            #line 170 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("ViewModel vm, string action = null)\r\n\t\t{\r\n\t\t\tif (\"delete\".EqualsIgnoreCase(action" +
                    "))\r\n\t\t\t{\r\n\t\t\t\t//if (!User.AnyAct(ACT.");
            
            #line 174 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Delete)) { throw new OrionException(\"權限不足!!\"); }\r\n\r\n\t\t\t\tSvc.");
            
            #line 176 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(".Delete(vm.");
            
            #line 176 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Name));
            
            #line default
            #line hidden
            this.Write(");\r\n\t\t\t\tthis.SetStatusSuccess(\"刪除成功!!\");\r\n\t\t\t\treturn View();\r\n\t\t\t}\r\n\r\n\t\t\tif (!Mod" +
                    "elState.IsValid) { return View(vm); }\r\n\r\n\t\t\t");
            
            #line 183 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain = toDomain(vm);\r\n\r\n\t\t\t");
            
            #line 185 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 185 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(" = Svc.");
            
            #line 185 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write(".Save(domain);\r\n\r\n\t\t\tthis.SetStatusSuccess(\"儲存成功!!\");\r\n\t\t\treturn RedirectToAction" +
                    "(nameof(Edit), new { ");
            
            #line 188 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ControllersTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(" });\r\n\t\t}\r\n\r\n\r\n\t}\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
