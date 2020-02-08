using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Orion.Mvc.Filters
{
    internal class JsonNetResult : JsonResult
    {
        static IList<JsonConverter> DefaultConverters = new List<JsonConverter>()
        {
            new StringEnumConverter(), 
            new IsoDateTimeConverter(),
            //new StringEnumConverter() { CamelCaseText = true },
        };

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            this.SerializerSettings = new JsonSerializerSettings()
            {
                Converters = DefaultConverters,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        public JsonNetResult(JsonResult original) : this()
        {
            this.ContentEncoding = original.ContentEncoding;
            this.ContentType = original.ContentType;
            this.JsonRequestBehavior = original.JsonRequestBehavior;
            this.Data = original.Data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null) { response.ContentEncoding = ContentEncoding; }

            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

            var serializer = JsonSerializer.Create(this.SerializerSettings);
            serializer.Serialize(writer, Data);

            writer.Flush();
        }
    }

}