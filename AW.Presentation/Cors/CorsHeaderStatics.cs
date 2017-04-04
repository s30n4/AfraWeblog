namespace AW.Presentation.Cors
{
    public class CorsHeaderStatics
    {
        /// <summary>
        /// return Access-Control-Allow-Origin header name.
        /// The origin parameter specifies a URI that may access the resource. The browser must enforce this.
        /// For requests without credentials, the server may specify "*" as a wildcard, thereby allowing any
        /// origin to access the resource.
        /// </summary>
        public static string Origin => "Access-Control-Allow-Origin";

        /// <summary>
        /// return Access-Control-Expose-Headers header name.
        /// The Access-Control-Expose-Headers header lets a server whitelist headers that browsers are allowed to access.
        /// </summary>
        public static string ExposeHeaders => "Access-Control-Expose-Headers";

        /// <summary>
        /// return Access-Control-Max-Age header name.
        /// The  Access-Control-Max-Age header indicates how long the results of a preflight request can be cached. For an example of a preflight request.
        /// </summary>
        public static string MaxAge => "Access-Control-Max-Age";

        /// <summary>
        /// return Access-Control-Allow-Credentials header name.
        /// The Access-Control-Allow-Credentials header Indicates whether or not the response to the request can be exposed when the credentials flag is true.
        /// When used as part of a response to a preflight request, this indicates whether or not the actual request can be made using credentials.
        /// Note that simple GET requests are not preflighted, and so if a request is made for a resource with credentials,
        /// if this header is not returned with the resource, the response is ignored by the browser and not returned to web content.
        /// </summary>
        public static string Credentials => "Access-Control-Allow-Credentials";

        /// <summary>
        /// return Access-Control-Allow-Methods header name.
        /// The Access-Control-Allow-Credentials header Indicates whether or not the response to the request can be exposed when the credentials flag is true.
        /// When used as part of a response to a preflight request, this indicates whether or not the actual request can be made using credentials. Note that
        /// simple GET requests are not preflighted, and so if a request is made for a resource with credentials, if this header is not returned with the resource,
        /// the response is ignored by the browser and not returned to web content.
        /// </summary>
        public static string Methods => "Access-Control-Allow-Methods";

        /// <summary>
        /// return Access-Control-Allow-Headers header name.
        /// The Access-Control-Allow-Headers header is used in response to a preflight request to indicate which HTTP headers can be used when making the actual request.
        /// </summary>
        public static string Headers => "Access-Control-Allow-Headers";
    }
}