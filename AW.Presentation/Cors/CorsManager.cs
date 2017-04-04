using System;
using System.Collections.Generic;
using AW.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AW.Presentation.Cors
{
    public class CorsManager : ICorsManager
    {
        private readonly HttpContext _context;
        private string _maxAge = "86400";
        private string _credential = "false";

        private readonly List<string> _origins = new List<string>();
        private readonly List<string> _exposedHeaders = new List<string>();
        private readonly List<string> _methods = new List<string>();
        private readonly List<string> _headers = new List<string>();

        public CorsManager(HttpContext httpContext)
        {
            _context = httpContext;
        }

        public ICorsManager HasExposeHeaders(params string[] headers)
        {
            foreach (var header in headers)
            {
                _headers.Add(header);
            }
            return this;
        }

        public ICorsManager WithOrigins(params string[] origins)
        {
            foreach (var origin in origins)
            {
                _origins.Add(origin);
            }
            return this;
        }

        public ICorsManager WithHeaders(params string[] headers)
        {
            foreach (var header in headers)
            {
                _headers.Add(header);
            }
            return this;
        }

        public ICorsManager WithExposedHeaders(params string[] headers)
        {
            foreach (var header in headers)
            {
                _exposedHeaders.Add(header);
            }
            return this;
        }

        public ICorsManager WithMethods(params string[] methods)
        {
            foreach (var method in methods)
            {
                _methods.Add(method);
            }
            return this;
        }

        public ICorsManager AllowCredentials()
        {
            _credential = "true";
            return this;
        }

        public ICorsManager DisallowCredentials()
        {
            _credential = "false";
            return this;
        }

        public ICorsManager AllowAnyOrigin()
        {
            _origins.Clear();
            _origins.Add("*");
            return this;
        }

        public ICorsManager AllowAnyMethod()
        {
            _methods.Clear();
            _methods.Add("*");
            return this;
        }

        public ICorsManager AllowAnyHeader()
        {
            _headers.Clear();
            _headers.Add("*");
            return this;
        }

        public ICorsManager SetPreflightMaxAge(TimeSpan preflightMaxAge)
        {
            _maxAge = Convert.ToString(preflightMaxAge);
            return this;
        }

        public void AddCorsOrigin()
        {
            _context.Response.Headers.Add(!string.IsNullOrEmpty(_maxAge)
                ? new KeyValuePair<string, StringValues>(CorsHeaderStatics.MaxAge, _maxAge)
                : new KeyValuePair<string, StringValues>(CorsHeaderStatics.MaxAge, "86400"));

            _context.Response.Headers.Add(new KeyValuePair<string, StringValues>(CorsHeaderStatics.Credentials, _credential));

            if (_exposedHeaders.Count > 0)
            {
                var headers = _exposedHeaders.Join();
                _context.Response.Headers.Add(new KeyValuePair<string, StringValues>(CorsHeaderStatics.ExposeHeaders, headers));
            }

            if (_methods.Count > 0)
            {
                var methods = _methods.Join();
                _context.Response.Headers.Add(new KeyValuePair<string, StringValues>(CorsHeaderStatics.Methods, methods));
            }

            if (_headers.Count <= 0) return;
            var serverHeaders = _headers.Join();
            _context.Response.Headers.Add(new KeyValuePair<string, StringValues>(CorsHeaderStatics.Headers, serverHeaders));
        }

        public void AddErrorException(SystemError errorContent)
        {
            _context.Response.WriteAsync(JsonConvert.SerializeObject(errorContent));
        }
    }
}