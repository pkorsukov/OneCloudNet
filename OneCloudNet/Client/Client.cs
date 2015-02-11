﻿using System;
using System.Net;
using OneCloudNet.Exceptions;
using OneCloudNet.Helpers;
using RestSharp;
using RestSharp.Deserializers;

namespace OneCloudNet.Client
{
    public partial class OneCloudNetClient : IOneCloudNetClient
    {
        /// <summary>
        /// Base URL for API requests.
        /// </summary>
        private const String ApiBaseUrl = "https://api.1cloud.ru";

        /// <summary>
        /// Unique private API token for authorization.
        /// </summary>
        private readonly String _token;

        /// <summary>
        /// Client for REST-communications.
        /// </summary>
        private RestClient _restClient;

        /// <summary>
        /// Helper for creating REST-requests.
        /// </summary>
        private RequestHelper _requestHelper;

        /// <summary>
        /// Proxy settings.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Default Constructor for the OneCloudNetClient
        /// </summary>
        /// <param name="token">The token to use for the 1Cloud Requests</param>
        /// <param name="proxy">The proxy to use for web requests</param>
        public OneCloudNetClient(String token, IWebProxy proxy = null)
        {
            Proxy = proxy;
            _token = token;
            LoadClient();
        }

        private void LoadClient()
        {
            _restClient = new RestClient(ApiBaseUrl);
            _restClient.Proxy = Proxy;
            _restClient.ClearHandlers();
            _restClient.AddHandler("*", new JsonDeserializer());

            _requestHelper = new RequestHelper(_token);
        }

        private T Execute<T>(IRestRequest request) where T : new()
        {
            var response = _restClient.Execute<T>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OneCloudRestException(response, HttpStatusCode.OK);

            return response.Data;
        }

        private IRestResponse Execute(IRestRequest request)
        {
            var response = _restClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new OneCloudRestException(response, HttpStatusCode.OK);

            return response;
        }

        private void ExecuteAsync(IRestRequest request, Action<IRestResponse> success, Action<OneCloudException> failure)
        {
            _restClient.ExecuteAsync(request, (response, asynchandle) =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    failure(new OneCloudRestException(response, HttpStatusCode.OK));
                }
                else
                {
                    success(response);
                }
            });
        }

        private void ExecuteAsync<T>(IRestRequest request, Action<T> success, Action<OneCloudException> failure)
            where T : new()
        {
            _restClient.ExecuteAsync<T>(request, (response, asynchandle) =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    failure(new OneCloudRestException(response, HttpStatusCode.OK));
                }
                else
                {
                    success(response.Data);
                }
            });
        }
    }
}