﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本: 16.0.0.0
//  
//     變更這個檔案可能會導致不正確的行為，而且如果已重新產生
//     程式碼，則會遺失變更。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CodeGenerator.Templates.Wcf
{
    using System.Linq;
    using CodeGenerator.Models;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ServiceTemplate : CodeGenerator.Templates.TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 4 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
 
	string className = TableMeta.Name + "Service";
	FilePath = TableMeta.NameSpace + ".Service.Impl/" + className + ".cs";


	var validateColumns = ActiveColumns
		.Where(x => !x.IsPrimaryKey)
		.Where(x => !x.IsEnum)
		.Where(x => !x.IsNullable)
		.Where(x => x.CodeType != "bool")
		.Where(x => x.Name != "RemarkText")
		;


            
            #line default
            #line hidden
            this.Write("using JustWin.API;\r\nusing JustWin.API.Models;\r\nusing System;\r\nusing System.Linq;\r" +
                    "\nusing System.Collections.Generic;\r\nusing System.Data.Linq;\r\nusing ");
            
            #line 24 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Dao;\r\nusing ");
            
            #line 25 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Domain;\r\nusing ");
            
            #line 26 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Domain.Enums;\r\n\r\n\r\nnamespace ");
            
            #line 29 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Service.Impl\r\n{\r\n\t/// <summary>");
            
            #line 31 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n\tpublic class ");
            
            #line 32 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(" : I");
            
            #line 32 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write("\r\n\t{\r\n\t\tprivate readonly I");
            
            #line 34 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Dao _");
            
            #line 34 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao;\r\n\r\n\t\tpublic ");
            
            #line 36 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write("(I");
            
            #line 36 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Dao ");
            
            #line 36 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao)\r\n\t\t{\r\n\t\t\t_");
            
            #line 38 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao = ");
            
            #line 38 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao;\r\n\t\t}\r\n\r\n\r\n\t\t//TODO 程式產生未完成\r\n\t\t//public Dictionary<int, string> Get");
            
            #line 43 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Dic(UseStatus? status)\r\n\t\t//{\r\n\t\t//    return _");
            
            #line 45 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.Get");
            
            #line 45 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Dic(status);\r\n\t\t//}\r\n\r\n\t\tpublic Pagination<");
            
            #line 48 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> GetPagination(WhereParams<");
            
            #line 48 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> findParam, PageParams<");
            
            #line 48 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> pageParams)\r\n\t\t{\r\n\t\t\tusing (OrionUtils.TransactionReadUncommitted())\r\n\t\t\t" +
                    "{\r\n\t\t\t\treturn _");
            
            #line 52 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.GetPagination(findParam, pageParams);\r\n\t\t\t}\r\n\t\t}\r\n\r\n\r\n\t\t//public List<");
            
            #line 57 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> GetList(WhereParams<");
            
            #line 57 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain> findParam)\r\n\t\t//{\r\n\t\t//\tusing (OrionUtils.TransactionReadUncommitted())\r\n" +
                    "\t\t//\t{\r\n\t\t//\t\treturn _");
            
            #line 61 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.GetList(findParam);\r\n\t\t//\t}\r\n\t\t//}\r\n\r\n\t\tpublic ");
            
            #line 65 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain GetById(");
            
            #line 65 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 65 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t{\r\n\t\t\t");
            
            #line 67 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain = _");
            
            #line 67 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.GetById(");
            
            #line 67 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n\t\t\tif (domain == null) { throw new OrionNoDataException(\"");
            
            #line 69 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("資料不存在\"); }\r\n\r\n\t\t\treturn domain;\r\n\t\t}\r\n\r\n\r\n\t\t//TODO 程式產生未完成\r\n\t\tpublic int Save(");
            
            #line 76 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Name));
            
            #line default
            #line hidden
            this.Write("Domain domain)\r\n\t\t{\r\n");
            
            #line 78 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
 foreach(var col in validateColumns) { 
            
            #line default
            #line hidden
            this.Write("\t\t\tChecker.Has(domain.");
            
            #line 79 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(", \"");
            
            #line 79 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.DisplayName));
            
            #line default
            #line hidden
            this.Write("不可以為空\");\r\n");
            
            #line 80 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
 } 
            
            #line default
            #line hidden
            
            #line 81 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
 foreach(var col in WithColumns("ModifyBy", "ModifiedBy")) { 
            
            #line default
            #line hidden
            this.Write("\t\t\tChecker.Has(domain.");
            
            #line 82 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(", \"修改人不可以為空\");\r\n");
            
            #line 83 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\treturn _");
            
            #line 85 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.Save(domain);\r\n\t\t}\r\n\r\n\t\tpublic void Delete(");
            
            #line 88 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 88 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(", int modifyBy)\r\n\t\t{\r\n\t\t\t_");
            
            #line 90 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Variable));
            
            #line default
            #line hidden
            this.Write("Dao.Delete(");
            
            #line 90 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Wcf\ServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.PK.Variable));
            
            #line default
            #line hidden
            this.Write(", modifyBy);\r\n\t\t}\r\n\t}\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
