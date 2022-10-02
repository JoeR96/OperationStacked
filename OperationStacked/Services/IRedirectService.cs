using System;
using Microsoft.AspNetCore.Authentication;

namespace OperationStacked.Services
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
