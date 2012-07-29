using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Moq;
using System.Collections.Specialized;

namespace Jumbleblocks.Testing.Web
{
    /// <summary>
    /// Mock helper for HttpRequest
    /// </summary>
    public static class HttpRequestHelper
    {
        /// <summary>
        /// sets up the request url on the HttpRequest
        /// </summary>
        /// <param name="request">HttpRequest (Moq-ed)</param>
        /// <param name="url">url to set</param>
        /// <exception cref="ArgumentNullException">request is null</exception>
        /// <exception cref="ArgumentNullException">url is null or empty</exception>
        /// <exception cref="ArgumentException">url </exception>
        /// <returns>Http Request Base</returns>
        public static HttpRequestBase SetupRequestUrl(this HttpRequestBase request, string url)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (String.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");

            if (!url.StartsWith("~/"))
                throw new ArgumentException("Must be a virtual url starting with \"~/\".", "url");

            var mock = Mock.Get(request);

            mock.Setup(req => req.QueryString)
                .Returns(GetQueryStringParameters(url));
            mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
                .Returns(GetUrlFullPath(url));
            mock.Setup(req => req.PathInfo)
                .Returns(string.Empty);

            return request;
        }

        /// <summary>
        /// Sets the referrer url
        /// </summary>
        /// <param name="request">HttpRequestBase</param>
        /// <param name="referrerUrl">url referrer</param>
        /// <returns>HttpRequestBase</returns>
        /// <exception cref="ArgumentNullExeption">request is null</exception>
        /// <exception cref="ArgumentNullExeption">referrerUrl is null or empty</exception>
        public static HttpRequestBase SetRequestReferrer(this HttpRequestBase request, string referrerUrl)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (String.IsNullOrWhiteSpace(referrerUrl))
                throw new ArgumentNullException("referrerUrl");

            var mock = Mock.Get(request);

            mock.Setup(r => r.UrlReferrer)
                .Returns(new Uri(referrerUrl));

            return request;
        }

        /// <summary>
        /// Gets the query string parameters from a url
        /// </summary>
        /// <param name="url">url to look at</param>
        /// <returns>NameValueCollection</returns>
        private static NameValueCollection GetQueryStringParameters(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");

            if (url.Contains("?"))
            {
                NameValueCollection parameters = new NameValueCollection();

                string[] parts = url.Split("?".ToCharArray());
                string[] keys = parts[1].Split("&".ToCharArray());

                foreach (string key in keys)
                {
                    string[] part = key.Split("=".ToCharArray());
                    parameters.Add(part[0], part[1]);
                }

                return parameters;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the path section from a url
        /// </summary>
        /// <param name="url">url to pull path from</param>
        /// <returns>path from url</returns>
        private static string GetUrlFullPath(string url)
        {
            if (url.Contains("?"))
                return url.Substring(0, url.IndexOf("?"));
            else
                return url;
        }

        /// <summary>
        /// Sets the request
        /// </summary>
        /// <param name="request">HttpRequest (Moq-ed)</param>
        /// <param name="httpMethod">Http method to use</param>
        /// <returns>HttpRequestBase</returns>
        /// <exception cref="ArgumentNullExeption">request is null</exception>
        /// <exception cref="ArgumentNullExeption">httpMethod is null or empty</exception>
        public static HttpRequestBase SetHttpMethod(this HttpRequestBase request, string httpMethod)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (String.IsNullOrWhiteSpace(httpMethod))
                throw new ArgumentNullException("httpMethod");

            Mock.Get(request)
                .Setup(req => req.HttpMethod)
                .Returns(httpMethod);

            return request;
        }

        /// <summary>
        /// Makes the request Ajax
        /// </summary>
        /// <param name="request">HttpRequest (Moq-ed)</param>
        /// <returns>HttpRequestBase</returns>
        /// <exception cref="ArgumentNullExeption">request is null</exception>
        public static HttpRequestBase SetRequestToBeAjax(this HttpRequestBase request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var mock = Mock.Get(request);

            mock.Setup(r => r["X-Requested-With"])
                .Returns("XMLHttpRequest");

            return request;
        }

        /// <summary>
        /// Adds cookie data to the request
        /// </summary>
        /// <param name="request">request to add cookiedata too</param>
        /// <param name="cookieName">name of cookie to add data to</param>
        /// <param name="key">key for data value</param>
        /// <param name="value">value for data</param>
        /// <returns>HttpRequestBase</returns>
        /// <exception cref="ArgumentNullExeption">request is null</exception>
        /// <exception cref="ArgumentNullExeption">cookieName is null or empty</exception>
        /// <exception cref="ArgumentNullExeption">key is null or empty</exception>
        /// <exception cref="ArgumentNullExeption">value is null</exception>
        public static HttpRequestBase AddCookieData(this HttpRequestBase request, string cookieName, string key, string value)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (String.IsNullOrWhiteSpace(cookieName))
                throw new ArgumentNullException("cookieName");

            if(String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            if(value == null)
                throw new ArgumentNullException("value");

            if(request.Cookies == null)
            {
                var cookieCollection = new HttpCookieCollection();
                Mock.Get(request).Setup(r => r.Cookies).Returns(cookieCollection);
            }

            if (!request.Cookies.AllKeys.Contains(cookieName))
                request.Cookies.Add(new HttpCookie(cookieName));

            request.Cookies[cookieName][key] = value;

            return request;
        }
    }
}
