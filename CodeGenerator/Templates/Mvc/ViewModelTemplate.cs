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
    
    #line 1 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ViewModelTemplate : CodeGenerator.Templates.TemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 4 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
 
	string className = TableMeta.Name + "ViewModel";
	FilePath = TableMeta.NameSpace + ".WebApp/Models/" + className + ".cs";

            
            #line default
            #line hidden
            this.Write("using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing Syste" +
                    "m.Runtime.Serialization;\r\nusing System.ComponentModel.DataAnnotations;\r\nusing Sy" +
                    "stem.Text;\r\nusing JustWin.API.Models;\r\nusing ");
            
            #line 15 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".Domain.Enums;\r\n\r\nnamespace ");
            
            #line 17 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.NameSpace));
            
            #line default
            #line hidden
            this.Write(".WebApp.Models \r\n{\r\n\t/// <summary>");
            
            #line 19 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(TableMeta.Description));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n\tpublic class ");
            
            #line 20 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(" \r\n\t{\r\n");
            
            #line 22 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
 foreach(var col in TableMeta.Columns) { 
            
            #line default
            #line hidden
            this.Write("\t\t/// <summary>");
            
            #line 23 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Description));
            
            #line default
            #line hidden
            this.Write("</summary>\r\n");
            
            #line 24 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
 if(!col.IsNullable && !col.IsPrimaryKey){ 
            
            #line default
            #line hidden
            this.Write("\t\t[Required]\r\n");
            
            #line 26 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t[Display(Name = \"");
            
            #line 27 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.DisplayName));
            
            #line default
            #line hidden
            this.Write("\")]\r\n\t\tpublic ");
            
            #line 28 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.CodeType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 28 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(col.Name));
            
            #line default
            #line hidden
            this.Write(" { get; set; }\r\n\r\n");
            
            #line 30 "D:\Jax-Work\Dropbox\JW-Project\CoreTpl\CodeGenerator\Templates\Mvc\ViewModelTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t}\r\n\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
}
