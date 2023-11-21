namespace Controller.Generator
{
    public static class Constants
    {
        private const string MvcNamespace = "Microsoft.AspNetCore.Mvc.";
        public const string ControllerAttribute = MvcNamespace + "ApiController";

        public const string HttpPostAttribute = MvcNamespace + "HttpPost";
        public const string HttpGetAttribute = MvcNamespace + "HttpGet";
        public const string HttpDeleteAttribute = MvcNamespace + "HttpDelete";
        public const string AllowAnonymousAttribute = "Microsoft.AspNetCore.Authorization.AllowAnonymous";
    }
}
