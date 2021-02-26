```csharp
// Microsoft.AspNetCore.Authorization.AuthorizationMiddleware
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AuthorizationMiddleware
{
	private const string SuppressUseHttpContextAsAuthorizationResource = "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource";

	private const string AuthorizationMiddlewareInvokedWithEndpointKey = "__AuthorizationMiddlewareWithEndpointInvoked";

	private static readonly object AuthorizationMiddlewareWithEndpointInvokedValue = new object();

	private readonly RequestDelegate _next;

	private readonly IAuthorizationPolicyProvider _policyProvider;

	public AuthorizationMiddleware(RequestDelegate next, IAuthorizationPolicyProvider policyProvider)
	{
		_next = next ?? throw new ArgumentNullException("next");
		_policyProvider = policyProvider ?? throw new ArgumentNullException("policyProvider");
	}

	public async Task Invoke(HttpContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		Endpoint endpoint = context.GetEndpoint();
		if (endpoint != null)
		{
			context.Items["__AuthorizationMiddlewareWithEndpointInvoked"] = AuthorizationMiddlewareWithEndpointInvokedValue;
		}
		IReadOnlyList<IAuthorizeData> authorizeData = endpoint?.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();
		AuthorizationPolicy policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData);
		if (policy == null)
		{
			await _next(context);
			return;
		}
		IPolicyEvaluator policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();
		AuthenticateResult authenticationResult = await policyEvaluator.AuthenticateAsync(policy, context);
		if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
		{
			await _next(context);
			return;
		}
		bool isEnabled;
		object resource = ((!(AppContext.TryGetSwitch("Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource", out isEnabled) && isEnabled)) ? ((object)context) : ((object)endpoint));
		PolicyAuthorizationResult authorizeResult = await policyEvaluator.AuthorizeAsync(policy, authenticationResult, context, resource);
		await context.RequestServices.GetRequiredService<IAuthorizationMiddlewareResultHandler>().HandleAsync(_next, context, policy, authorizeResult);
	}
}

```