using DockerTestsSample.Common.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Http;

namespace DockerTestsSample.Api.IntegrationTests.Infrastructure;

/// <summary>
/// Filter that checks all HttpClient requests to redirect the necessary ones to the TestServer
/// </summary>
internal sealed class TestServerMessageFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly TestServer _server;

    public TestServerMessageFilter(TestServer server)
    {
        _server = server;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            // Add other handlers first
            next(builder);

            // TestServerHandler should be the last one
            var serverHandler = new TestServerHandler(_server);
            builder.AdditionalHandlers.Add(serverHandler);
        };
    }

    private sealed class TestServerHandler : DelegatingHandler
    {
        private readonly string _serverAuthority;
        private readonly HttpMessageHandlerInvoker _serverHandler;

        public TestServerHandler(TestServer server)
        {
            _serverAuthority = GetAuthority(server.BaseAddress).Required();
            var testServerHandler = server.CreateHandler();

            _serverHandler = new HttpMessageHandlerInvoker(testServerHandler);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return IsRequestToTestServer(request)
                // Redirect to a special stub handler
                ? _serverHandler.ExecuteAsync(request, cancellationToken)
                // Redirect to the real network
                : base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// If the request is directed to the test server
        /// </summary>
        private bool IsRequestToTestServer(HttpRequestMessage request)
        {
            return GetAuthority(request.RequestUri) == _serverAuthority;
        }

        private static string? GetAuthority(Uri? uri) => uri?.GetLeftPart(UriPartial.Authority);
    }

    private sealed class HttpMessageHandlerInvoker : DelegatingHandler
    {
        public HttpMessageHandlerInvoker(HttpMessageHandler messageHandler)
        {
            InnerHandler = messageHandler;
        }

        public Task<HttpResponseMessage> ExecuteAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            => SendAsync(request, cancellationToken);
    }
}
