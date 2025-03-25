
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ScreenSound.Web.Services;

public class CookieHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include); // aqui estou falando para ele enviar todos os cookies para as outras paginas
       return base.SendAsync(request, cancellationToken);
    }
}
