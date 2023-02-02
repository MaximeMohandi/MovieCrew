﻿namespace BillB0ard_API.Domain.Exception
{
    public class RateException : System.Exception
    {
        public RateException(string message) : base(message) { }
    }

    public class RateLimitException : RateException
    {
        public RateLimitException(decimal rate) : base($"The rate must be between 0 and 10. Actual : {rate}") { }
    }
}
