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

namespace WebApiSelfHost.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Common.Logging;

    public class ValuesController : ApiController
    {
        static readonly ILog s_log = LogManager.GetLogger<ValuesController>();
        // GET api/values 
        public IEnumerable<string> Get()
        {
            s_log.Info("Loading values...");
            return new[] {"value1", "value2"};
        }


        // GET api/values/5 
        public string Get(int id)
        {
            s_log.InfoFormat("Loading value {0}", id);
            return "value" + id;
        }


        // POST api/values 
        public void Post([FromBody] string value)
        {
            s_log.InfoFormat("Adding value '{0}'", value);
        }


        // PUT api/values/5 
        public void Put(int id, [FromBody] string value)
        {
            s_log.InfoFormat("Updating value {0} with '{1}'", id, value);
        }


        // DELETE api/values/5 
        public void Delete(int id)
        {
            s_log.InfoFormat("Deleting value {0}", id);
        }
    }
}