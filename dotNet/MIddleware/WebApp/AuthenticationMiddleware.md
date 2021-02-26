```chsarp
// Microsoft.AspNetCore.Authentication.AuthenticationMiddleware
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AuthenticationMiddleware
{
	private readonly RequestDelegate _next;

	public IAuthenticationSchemeProvider Schemes
	{
		get;
		set;
	}

	public AuthenticationMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
	{
		if (next == null)
		{
			throw new ArgumentNullException("next");
		}
		if (schemes == null)
		{
			throw new ArgumentNullException("schemes");
		}
		_next = next;
		Schemes = schemes;
	}

	public async Task Invoke(HttpContext context)
	{
		context.Features.Set((IAuthenticationFeature)new AuthenticationFeature
		{
			OriginalPath = context.Request.Path,
			OriginalPathBase = context.Request.PathBase
		});
		IAuthenticationHandlerProvider handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
		foreach (AuthenticationScheme item in await Schemes.GetRequestHandlerSchemesAsync())
		{
			IAuthenticationRequestHandler authenticationRequestHandler = (await handlers.GetHandlerAsync(context, item.Name)) as IAuthenticationRequestHandler;
			bool flag = authenticationRequestHandler != null;
			if (flag)
			{
				flag = await authenticationRequestHandler.HandleRequestAsync();
			}
			if (flag)
			{
				return;
			}
		}
		AuthenticationScheme authenticationScheme = await Schemes.GetDefaultAuthenticateSchemeAsync();
		if (authenticationScheme != null)
		{
			AuthenticateResult authenticateResult = await context.AuthenticateAsync(authenticationScheme.Name);
			if (authenticateResult?.Principal != null)
			{
				context.User = authenticateResult.Principal;
			}
		}
		await _next(context);
	}
}

```