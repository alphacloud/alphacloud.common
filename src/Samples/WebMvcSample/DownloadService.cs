#region copyright

// Copyright 2013-2015 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

namespace WebMvcSample
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Logging;
    using JetBrains.Annotations;

    public class DownloadService
    {
        static readonly ILog s_log = LogManager.GetLogger<DownloadService>();
        readonly Uri _uri;


        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public DownloadService([NotNull] Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            _uri = uri;
        }


        public async Task<string> Download()
        {
            using (log4net.LogicalThreadContext.Stacks["NDC"].Push("Download"))
            {
                s_log.InfoFormat("Preparing to download {0}", _uri);
                var req = WebRequest.CreateHttp(_uri);

                var resp = await req.GetResponseAsync().ConfigureAwait(false);
                s_log.InfoFormat("Request sent");
                using (var s = resp.GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var str = await sr.ReadToEndAsync().ConfigureAwait(false);
                        s_log.Info("response received");
                        return str;
                    }
                }
            }
        }
    }
}