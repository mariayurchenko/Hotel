using SB.Shared.EntityProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SB.Shared
{
    public static class WebUtilities
    {
        public const int DefaultTimeout = 120000;

        public static string Post(string url, string body, Dictionary<string, string> headers = null, int timeout = DefaultTimeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "POST";
            request.Timeout = timeout;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/json";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(body);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            return GetResponse(response);
        }

        public static string Post(string url, string token, string body)
        {
            return SendData(url, token, body, "POST");
        }

        public static string Get(string url, string token)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "GET";
            request.Headers.Set(HttpRequestHeader.Authorization, token);
            request.ServicePoint.Expect100Continue = false;

            var response = (HttpWebResponse)request.GetResponse();

            return GetResponse(response);
        }

        public static string Get(string url, string login, string password)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "GET";
            request.Headers.Set(HttpRequestHeader.Authorization, SBCustomSettings.GetBasicHeader(login, password));
            request.ServicePoint.Expect100Continue = false;

            var response = (HttpWebResponse)request.GetResponse();

            return GetResponse(response);
        }

        public static string GetResponse(HttpWebResponse webResponse)
        {
            string response;
            var receiveStream = webResponse.GetResponseStream();

            using (var readStream = new StreamReader(receiveStream ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                response = readStream.ReadToEnd();
            }

            return response;
        }

        public static string Put(string url, string token, string body)
        {
            return SendData(url, token, body, "PUT");
        }

        private static string SendData(string url, string token, string body, string method, int timeout = DefaultTimeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = method;
            request.Timeout = timeout;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Headers.Set(HttpRequestHeader.Authorization, token);
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(body);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            return GetResponse(response);
        }

        public static string Delete(string url, string token, string body)
        {
            return SendData(url, token, body, "DELETE");
        }
    }
}