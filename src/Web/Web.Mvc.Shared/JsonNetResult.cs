namespace Alphacloud.Common.Web.Mvc
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using JetBrains.Annotations;
    using Newtonsoft.Json;


    /// <summary>
    /// JSon.Net formatted result.
    /// </summary>
    [PublicAPI]
    public class JsonNetResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        public JsonNetResult()
            : this(null)
        {}


        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        /// <param name="data">Data to format.</param>
        /// <param name="contentType">HTTP Content type.</param>
        /// <param name="encoding">The encoding.</param>
        public JsonNetResult(object data, string contentType= null, Encoding encoding = null)
        {
            SerializerSettings = new JsonSerializerSettings();
            Data = data;
            ContentType = contentType;
            ContentEncoding = encoding;
        }


        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        public Encoding ContentEncoding { get; set; }
        /// <summary>
        /// Gets or sets the HTTP content type value.
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the data to serialize.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the JSon formatting settings.
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Gets or sets the Json serializer settings.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; }


        /// <summary>
        /// Serializes data to Json.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = (!string.IsNullOrEmpty(ContentType)) ? ContentType : "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) {
                    Formatting = Formatting
                };
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}
