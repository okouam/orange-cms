using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CodeKinden.OrangeCMS.Domain.Models;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
{
    class API
    {
        public API(Role role) : this()
        {
            var username = "qa-" + role.ToString("g").ToLower();
            GetAccessToken(username, username + "-password");
        }

        public API()
        {
            restClient = new RestClient(ConfigurationManager.AppSettings["regression.url"]);
        }

        public IRestResponse Post(string uri, object json)
        {
            return Post(uri, x => WithJsonBody(json, x));
        }

        public IRestResponse PostJson(string uri, object json = null, NameValueCollection query = null, NameValueCollection headers = null)
        {
            return Post(uri, x => WithJsonBody(json, x), query, headers);
        }

        public IRestResponse PostForm(string uri, NameValueCollection form = null, NameValueCollection query = null, NameValueCollection headers = null)
        {
            return Post(uri, x => WithFormBody(form, x), query, headers);
        }

        public IRestResponse PostFile(string uri, string resourceName)
        {
            return Post(uri, x => WithFile(resourceName, x));
        }

        public IRestResponse Delete(string uri)
        {
            var request = CreateWebApplicationRequest(uri, Method.DELETE);
            return restClient.Delete(request);
        }

        public IRestResponse Get(string uri, object query = null, NameValueCollection headers = null)
        {
            var request = CreateWebApplicationRequest(uri, Method.GET);

            WithHeaders(headers, request);

            if (query != null)
            {
                request.AddObject(query);
            }
       
            return restClient.Get(request);
        }

        private static void WithHeaders(NameValueCollection headers, IRestRequest request)
        {
            if (headers != null)
            {
                foreach (string key in headers)
                {
                    request.AddHeader(key, headers[key]);
                }
            }
        }

        public void GetAccessToken(string username, string password)
        {
            if (!availableAccessTokens.ContainsKey(username))
            {
                var response = PostForm("/tokens", new NameValueCollection
                {
                    {"username", username},
                    {"password", password},
                    {"grant_type", "password"}
                });

                var content = (JObject)JsonConvert.DeserializeObject(response.Content);
                availableAccessTokens.Add(username, content.GetValue("access_token").Value<string>());
            }

            accessToken = availableAccessTokens[username];
        }

        private RestRequest CreateWebApplicationRequest(string uri, Method method)
        {
            var request = new RestRequest(uri, method);

            if (accessToken != null)
            {
                request.AddHeader("Authorization", "Bearer " + accessToken);
            }

            return request;
        }

        private IRestResponse Post(string uri, Action<RestRequest> extraProcessing, NameValueCollection query = null, NameValueCollection headers = null)
        {
            var request = CreateWebApplicationRequest(uri, Method.POST);

            extraProcessing(request);

            WithQueryParameters(query, request);

            WithHeaders(headers, request);

            return restClient.Post(request);
        }

        private static void WithFormBody(NameValueCollection form, IRestRequest request)
        {
            if (form != null)
            {
                foreach (string key in form)
                {
                    request.AddParameter(key, form[key]);
                }
            }
        }

        private static void WithFile(string resourceName, RestRequest request)
        {

            if (!string.IsNullOrEmpty(resourceName))
            {
                var resourceFullName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(x => x.EndsWith(resourceName));
                var fileContents = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName);
                request.AddFile(resourceName, stream => fileContents.CopyTo(stream), resourceName, "text/csv");
            }
        }

        private static void WithQueryParameters(NameValueCollection query, IRestRequest request)
        {
            if (query != null)
            {
                foreach (string key in query)
                {
                    request.AddQueryParameter(key, query[key]);
                }
            }
        }

        private static void WithJsonBody(object json, IRestRequest request)
        {
            if (json != null)
            {
                request.AddJsonBody(json);
            }
        }

        private string accessToken;
        private static readonly IDictionary<string, string> availableAccessTokens = new Dictionary<string, string>(); 
        private readonly RestClient restClient;
    }
}