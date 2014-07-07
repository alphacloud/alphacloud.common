namespace KudaNado.Web.Mvc
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;


    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
            : this(null, null, null)
        {}


        public JsonNetResult(object data)
            : this(data, null, null)
        {}


        public JsonNetResult(object data, string contentType)
            : this(data, contentType, null)
        {}


        public JsonNetResult(object data, string contentType, Encoding encoding)
        {
            SerializerSettings = new JsonSerializerSettings();
            Data = data;
            ContentType = contentType;
            ContentEncoding = encoding;
        }


        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        [CLSCompliant(false)]
        public Formatting Formatting { get; set; }

        [CLSCompliant(false)]
        public JsonSerializerSettings SerializerSettings { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = ((!string.IsNullOrEmpty(ContentType)) ? ContentType : "application/json");
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) {
                    Formatting = Formatting
                };
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}
