using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppService
{
    public class RestRequest
    {
        public static string BASEURL = System.Configuration.ConfigurationManager.AppSettings["UrlSgqService"];
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }

        private async static Task<RestRequest> ToResponse(HttpResponseMessage result)
        {
            return new RestRequest()
            {
                StatusCode = result.StatusCode,
                Response = await result.Content.ReadAsStringAsync()
            };
        }

        public async static Task<RestRequest> Post(string uri, object obj, string token = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(obj);

                    if (!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // envia a requisição POST
                    var result = await client.PostAsync(BASEURL+uri, content);
                    // processa a resposta
                    return await RestRequest.ToResponse(result);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("error: " + ex);
                return null;
            }
        }

        public async static Task<RestRequest> Get(string uri, string token = null)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    if (!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }

                    // envia a requisição POST
                    var result = client.GetAsync(BASEURL+uri).Result;
                    // processa a resposta
                    return await RestRequest.ToResponse(result);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("error: " + ex);
                return null;
            }
        }

        public async static Task<RestRequest> Put(string uri, object obj, string token = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(obj);

                    if (!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // envia a requisição POST
                    var result = client.PostAsync(BASEURL+uri, content).Result;
                    // processa a resposta
                    return await RestRequest.ToResponse(result);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("error: " + ex);
                return null;
            }
        }

        public async static Task<RestRequest> Delete(string uri, string token = null)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    if (!String.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }

                    // envia a requisição POST
                    var result = client.DeleteAsync(BASEURL+uri).Result;
                    // processa a resposta
                    return await RestRequest.ToResponse(result);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("error: " + ex);
                return null;
            }
        }
    }
}
