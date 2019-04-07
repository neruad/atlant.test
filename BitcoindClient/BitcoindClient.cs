using BitcoindClient.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BitcoindClient
{
    public class BitcoindClient
    {
        private string _url { get; set; }
        private string _user { get; set; }
        private string _pwd { get; set; }

        public BitcoindClient()
        {
            _url = ConfigurationManager.AppSettings["BitcoindAddress"].ToString();
            _user = ConfigurationManager.AppSettings["UserName"].ToString();
            _pwd = ConfigurationManager.AppSettings["Password"].ToString();
        }

        public BitcoindClient(string url)
        {
            _url = url;
        }

        public string MakeRequest(string jsonRequest)
        {
            try
            {
                var tempCookies = new CookieContainer();
                var encoding = new ASCIIEncoding();
                var byteData = encoding.GetBytes(jsonRequest);
                var postReq = (HttpWebRequest)WebRequest.Create(_url);
                postReq.Credentials = new NetworkCredential(_user, _pwd);
                postReq.Method = "POST";
                postReq.KeepAlive = true;
                postReq.CookieContainer = tempCookies;
                postReq.ContentType = "application/json";
                postReq.ContentLength = byteData.Length;
                var postreqstream = postReq.GetRequestStream();
                postreqstream.Write(byteData, 0, byteData.Length);
                postreqstream.Close();
                var postresponse = (HttpWebResponse)postReq.GetResponse();
                var postreqreader = new StreamReader(postresponse.GetResponseStream());
                return postreqreader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public T MakeRequest<T>(Methods method, params object[] parameters)
        {
            var jsonRpcRequest = new JsonRpcRequest(1, method.ToString(), parameters);
            var webRequest = (HttpWebRequest)WebRequest.Create(_url);
            SetBasicAuthHeader(webRequest, _user, _pwd);
            webRequest.Credentials = new NetworkCredential(_user, _pwd);
            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";
            webRequest.Proxy = null;
            webRequest.Timeout = 3000;
            var byteArray = jsonRpcRequest.GetBytes();
            webRequest.ContentLength = jsonRpcRequest.GetBytes().Length;

            using (var dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Dispose();
            }

            try
            {
                string json;

                using (var webResponse = webRequest.GetResponse())
                {
                    using (var stream = webResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            reader.Dispose();
                            json = result;
                        }
                    }
                }

                var rpcResponse = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(json);
                return rpcResponse.Result;
            }
            catch (WebException webException)
            {
                var webResponse = webException.Response as HttpWebResponse;

                if (webResponse != null)
                {
                    switch (webResponse.StatusCode)
                    {
                        case HttpStatusCode.InternalServerError:
                            {
                                using (var stream = webResponse.GetResponseStream())
                                {
                                    if (stream == null)
                                    {
                                        throw new Exception("The RPC request was either not understood by the server or there was a problem executing the request", webException);
                                    }

                                    using (var reader = new StreamReader(stream))
                                    {
                                        var result = reader.ReadToEnd();
                                        reader.Dispose();

                                        try
                                        {
                                            var jsonRpcResponseObject = JsonConvert.DeserializeObject<JsonRpcResponse<object>>(result);

                                            //var internalServerErrorException = new Exception(jsonRpcResponseObject.Error.Message, webException)
                                            //{
                                            //    RpcErrorCode = jsonRpcResponseObject.Error.Code
                                            //};

                                            //throw internalServerErrorException;
                                            throw new Exception(result, webException);
                                        }
                                        catch (JsonException)
                                        {
                                            throw new Exception(result, webException);
                                        }
                                    }
                                }
                            }

                        default:
                            throw new Exception("Unhandled web exception", webException);
                    }
                }
                
                if (webException.Message == "The operation has timed out")
                {
                    throw new Exception(webException.Message);
                }
                
                throw new Exception("An unknown web exception occured while trying to read the response", webException);
            }
            catch (Exception ex)
            {
                throw new Exception($"A problem was encountered while calling MakeRpcRequest() for: {jsonRpcRequest.Method}", ex);
            }
        }

        private static void SetBasicAuthHeader(WebRequest webRequest, string username, string password)
        {
            var authInfo = username + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            webRequest.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}
