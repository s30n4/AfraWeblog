using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AW.Cors
{
    public class CorsService : ICorsService
    {
        private readonly HttpContext _context;

        public CorsService(HttpContext context)
        {
            //if (context == null)
            //{
            //    throw new ArgumentNullException(nameof(context));
            //}
            _context = context;
        }

        /// <summary>
        /// Looks up a policy using the <paramref name="policyName"/> and then evaluates the policy using the passed in context.
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns>A <see cref="CorsResult"/> which contains the result of policy evaluation and can be
        /// used by the caller to set appropriate response headers.</returns>
        public CorsResult EvaluatePolicy(string policyName)
        {
            var policy = CorsOptions.GetPolicy(policyName);
            return EvaluatePolicy(policy);
        }

        /// <summary>
        /// evaluates the <paramref name="policy"/> using the passed in context.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>A <see cref="CorsResult"/> which contains the result of policy evaluation and can be
        /// used by the caller to set appropriate response headers.</returns>
        public CorsResult EvaluatePolicy(CorsPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            var corsResult = new CorsResult();
            var accessControlRequestMethod = _context.Request.Headers[CorsConstants.AccessControlRequestMethod];
            if (string.Equals(_context.Request.Method, CorsConstants.PreflightHttpMethod, StringComparison.OrdinalIgnoreCase) &&
                !StringValues.IsNullOrEmpty(accessControlRequestMethod))
            {
                EvaluatePreflightRequest(_context, policy, corsResult);
            }
            else
            {
                EvaluateRequest(_context, policy, corsResult);
            }

            return corsResult;
        }

        /// <summary>
        /// Build a policy using <paramref name="configurePolicy"/>and then evaluates the policy using the passed in context
        /// </summary>
        /// <param name="configurePolicy"></param>
        /// <returns>A <see cref="CorsResult"/> which contains the result of policy evaluation and can be
        /// used by the caller to set appropriate response headers.</returns>
        public CorsResult EvaluatePolicy(Action<CorsPolicyBuilder> configurePolicy)
        {
            if (configurePolicy == null)
            {
                throw new ArgumentNullException(nameof(configurePolicy));
            }

            var policyBuilder = new CorsPolicyBuilder();
            configurePolicy(policyBuilder);
            var policy = policyBuilder.Build();

            var corsResult = new CorsResult();
            var accessControlRequestMethod = _context.Request.Headers[CorsConstants.AccessControlRequestMethod];
            if (string.Equals(_context.Request.Method, CorsConstants.PreflightHttpMethod, StringComparison.OrdinalIgnoreCase) &&
                !StringValues.IsNullOrEmpty(accessControlRequestMethod))
            {
                EvaluatePreflightRequest(_context, policy, corsResult);
            }
            else
            {
                EvaluateRequest(_context, policy, corsResult);
            }

            return corsResult;
        }

        /// <summary>
        /// Add <paramref name="policy"/> to service with <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="policy"></param>
        public static void Add(string name, CorsPolicy policy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            CorsOptions.AddPolicy(name, policy);
        }

        /// <summary>
        /// Build policy get from <paramref name="configurePolicy"/> and ten save to service with <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configurePolicy"></param>
        public static void Add(string name, Action<CorsPolicyBuilder> configurePolicy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configurePolicy == null)
            {
                throw new ArgumentNullException(nameof(configurePolicy));
            }

            CorsOptions.AddPolicy(name, configurePolicy);
        }

        /// <summary>
        /// Add <paramref name="policy"/> to service with <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="policy"></param>
        public void AddCors(string name, CorsPolicy policy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }
            CorsOptions.AddPolicy(name, policy);
        }

        /// <summary>
        /// Build policy get from <paramref name="configurePolicy"/> and ten save to service with <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configurePolicy"></param>
        public void AddCors(string name, Action<CorsPolicyBuilder> configurePolicy)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (configurePolicy == null)
            {
                throw new ArgumentNullException(nameof(configurePolicy));
            }

            CorsOptions.AddPolicy(name, configurePolicy);
        }

        /// <summary>
        /// apply <paramref name="result"/> to <paramref />
        /// </summary>
        /// <param name="result"></param>
        public void ApplyCors(CorsResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            var headers = _context.Response.Headers;

            if (result.AllowedOrigin != null)
            {
                headers[CorsConstants.AccessControlAllowOrigin] = result.AllowedOrigin;
            }

            if (result.VaryByOrigin)
            {
                headers["Vary"] = "Origin";
            }

            if (result.SupportsCredentials)
            {
                headers[CorsConstants.AccessControlAllowCredentials] = "true";
            }

            if (result.AllowedMethods.Count > 0)
            {
                // Filter out simple methods
                var nonSimpleAllowMethods = result.AllowedMethods
                    .Where(m =>
                        !CorsConstants.SimpleMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowMethods.Length > 0)
                {
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlAllowMethods,
                        nonSimpleAllowMethods);
                }
            }

            if (result.AllowedHeaders.Count > 0)
            {
                // Filter out simple request headers
                var nonSimpleAllowRequestHeaders = result.AllowedHeaders
                    .Where(header =>
                        !CorsConstants.SimpleRequestHeaders.Contains(header, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowRequestHeaders.Length > 0)
                {
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlAllowHeaders,
                        nonSimpleAllowRequestHeaders);
                }
            }

            if (result.AllowedExposedHeaders.Count > 0)
            {
                // Filter out simple response headers
                var nonSimpleAllowResponseHeaders = result.AllowedExposedHeaders
                    .Where(header =>
                        !CorsConstants.SimpleResponseHeaders.Contains(header, StringComparer.OrdinalIgnoreCase))
                    .ToArray();

                if (nonSimpleAllowResponseHeaders.Length > 0)
                {
                    headers.SetCommaSeparatedValues(
                        CorsConstants.AccessControlExposeHeaders,
                        nonSimpleAllowResponseHeaders);
                }
            }

            if (result.PreflightMaxAge.HasValue)
            {
                headers[CorsConstants.AccessControlMaxAge]
                    = result.PreflightMaxAge.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void EvaluateRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            if (!IsOriginAllowed(policy, origin))
            {
                return;
            }

            AddOriginToResult(origin, policy, result);
            result.SupportsCredentials = policy.SupportsCredentials;
            AddHeaderValues(result.AllowedExposedHeaders, policy.ExposedHeaders);
        }

        private void EvaluatePreflightRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            if (!IsOriginAllowed(policy, origin))
            {
                return;
            }

            var accessControlRequestMethod = context.Request.Headers[CorsConstants.AccessControlRequestMethod];
            if (StringValues.IsNullOrEmpty(accessControlRequestMethod))
            {
                return;
            }

            var requestHeaders =
                context.Request.Headers.GetCommaSeparatedValues(CorsConstants.AccessControlRequestHeaders);

            if (!policy.AllowAnyMethod)
            {
                var found = policy.Methods.Any(method => string.Equals(method, accessControlRequestMethod, StringComparison.OrdinalIgnoreCase));

                if (!found)
                {
                    return;
                }
            }

            if (!policy.AllowAnyHeader &&
                requestHeaders != null)
            {
                if (requestHeaders.Any(requestHeader => !CorsConstants.SimpleRequestHeaders.Contains(requestHeader, StringComparer.OrdinalIgnoreCase) &&
                                                        !policy.Headers.Contains(requestHeader, StringComparer.OrdinalIgnoreCase)))
                {
                    return;
                }
            }

            AddOriginToResult(origin, policy, result);
            result.SupportsCredentials = policy.SupportsCredentials;
            result.PreflightMaxAge = policy.PreflightMaxAge;
            result.AllowedMethods.Add(accessControlRequestMethod);
            if (policy.AllowAnyHeader)
                AddHeaderValues(result.AllowedHeaders, new[] { "*" });
            else
                AddHeaderValues(result.AllowedHeaders, requestHeaders);
        }

        private void AddOriginToResult(string origin, CorsPolicy policy, CorsResult result)
        {
            if (policy.AllowAnyOrigin)
            {
                if (policy.SupportsCredentials)
                {
                    result.AllowedOrigin = origin;
                    result.VaryByOrigin = true;
                }
                else
                {
                    result.AllowedOrigin = CorsConstants.AnyOrigin;
                }
            }
            else if (policy.IsOriginAllowed(origin))
            {
                result.AllowedOrigin = origin;

                if (policy.Origins.Count > 1)
                {
                    result.VaryByOrigin = true;
                }
            }
        }

        private void AddHeaderValues(IList<string> target, IEnumerable<string> headerValues)
        {
            if (headerValues == null)
            {
                return;
            }

            foreach (var current in headerValues)
            {
                if (current != "Content-Type")
                {
                    target.Add(current);
                }
            }
        }

        private bool IsOriginAllowed(CorsPolicy policy, StringValues origin)
        {
            if (StringValues.IsNullOrEmpty(origin))
            {
                return false;
            }

            if (policy.AllowAnyOrigin || policy.IsOriginAllowed(origin))
            {
                return true;
            }
            return false;
        }
    }
}