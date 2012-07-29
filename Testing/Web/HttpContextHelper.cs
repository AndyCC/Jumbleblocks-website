//http://www.hanselman.com/blog/ASPNETMVCSessionAtMix08TDDAndMvcMockHelpers.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Moq;

namespace Jumbleblocks.Testing.Web
{
    public static class HttpContextHelper
    {
        /// <summary>
        /// Creates a new Mocked HTTPContextBase
        /// </summary>
        /// <returns>HttpContextBase (Moq)</returns>
        public static HttpContextBase CreateMockedHttpContextBase()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }
    }
}
