using System;

namespace H3WebAPI.Models
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ApiVersionAttribute : Attribute
    {
        public ApiVersionAttribute(string majorVersion = "1", string minorVersion = "0", ApiVersionProductType product = ApiVersionProductType.DefaultAPI)
        {
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            Product = product;
        }

        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
        public ApiVersionProductType Product { get; set; }
    }

    public enum ApiVersionProductType
    {
        DefaultAPI = 0,
        InternalAPI = 1,
        ExternalAPI = 2
    }
}
