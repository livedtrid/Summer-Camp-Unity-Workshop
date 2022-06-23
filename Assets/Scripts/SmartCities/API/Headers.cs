namespace SmartCities.API
{
    public static class Headers
    {
        public class Key
        {
            public const string ContentType = @"Content-Type";
            public const string XCSRFToken = @"X-CSRF-Token";
            public const string Authorization = @"Authorization";
            public const string Accept = @"Accept";
            public const string ApiKey = @"api_key";
            
        }

        public class Value
        {
            public const string Json = @"application/json";
            public const string Fetch = @"FETCH";
            public const string BasicAuthentication = @"Basic Og==";
        }
    }

}