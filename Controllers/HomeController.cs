using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ES_API.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HttpClient client;

        HttpResponseMessage response;

        public HomeController()
        {
            client = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } });
            client.BaseAddress = new Uri("https://34.236.95.15:9200/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("elastic:123456")));
        }

        [HttpGet]
        [Route("GetIndexes")]
        public ContentResult GetIndexes()
        {
            try
            {
                response = client.GetAsync("_cat/indices").Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":" + "[]" + "}",
                        ContentType = "application/json"
                    };
                }
                var arrayOfIndexAsString = response.Content.ReadAsStringAsync().Result;
                var jsonReturnAsString = "{\"data\":" + arrayOfIndexAsString + "}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":" + "[]" + "}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpDelete]
        [Route("DeleteIndex/{index}")]
        public ContentResult DeleteIndex(string index)
        {
            try
            {
                response = client.DeleteAsync(index).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":\"error\"}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonReturnAsString = "{\"data\":\"success\"}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":\"error\"}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpPost]
        [Route("CreateIndex/{index}")]
        public ContentResult CreateIndex(string index)
        {
            try
            {
                response = client.PutAsync(index, null).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":\"error\"}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonReturnAsString = "{\"data\":\"success\"}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":\"error\"}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpGet]
        [Route("GetDocuments/{index}")]
        public ContentResult GetDocuments(string index)
        {
            try
            {
                response = client.GetAsync(index + "/_search").Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":" + "[]" + "}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                int pos1 = jsonResultAsString.IndexOf("[");
                int pos2 = jsonResultAsString.IndexOf("]");
                var jsonReturnAsString = "{\"data\":" + jsonResultAsString.Substring(pos1, pos2 - pos1 + 1) + "}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":" + "[]" + "}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpGet]
        [Route("SearchDocuments/{index}/{query}")]
        public ContentResult SearchDocuments(string index, string query)
        {
            try
            {
                var content = "{\"query\":{\"query_string\":{\"query\":\"" + query + "\"}}}";
                response = client.PostAsync(index + "/_search", new StringContent(content, Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":" + "[]" + "}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                int pos1 = jsonResultAsString.IndexOf("[");
                int pos2 = jsonResultAsString.IndexOf("]");
                var jsonReturnAsString = "{\"data\":" + jsonResultAsString.Substring(pos1, pos2 - pos1 + 1) + "}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":" + "[]" + "}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpDelete]
        [Route("DeleteDocument/{index}/{id}")]
        public ContentResult DeleteDocument(string index, string id)
        {
            try
            {
                response = client.DeleteAsync(index + "/_doc/" + id).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":\"error\"}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonReturnAsString = "{\"data\":\"success\"}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":\"error\"}",
                    ContentType = "application/json"
                };
            }
        }

        [HttpPost]
        [Route("AddDocument/{index}/{jsonDocumentAsString}")]
        public ContentResult AddDocument(string index, string jsonDocumentAsString)
        {
            try
            {
                response = client.PostAsync(index + "/_doc", new StringContent(jsonDocumentAsString, Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        Content = "{\"data\":\"error\"}",
                        ContentType = "application/json"
                    };
                }
                var jsonResultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonReturnAsString = "{\"data\":\"success\"}";
                return new ContentResult
                {
                    Content = jsonReturnAsString,
                    ContentType = "application/json"
                };
            }
            catch
            {
                return new ContentResult
                {
                    Content = "{\"data\":\"error\"}",
                    ContentType = "application/json"
                };
            }
        }
    }
}
