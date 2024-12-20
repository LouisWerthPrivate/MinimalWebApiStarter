using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace APITemplate.Authentication
{
    /// <summary>
    /// Options for API Key Authentication.
    /// </summary>
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Gets or sets the name of the header that contains the API key.
        /// </summary>
        public string TokenHeaderName { get; set; } = "X-Api-Key";

        /// <summary>
        /// Gets or sets the allowed API keys, separated by commas.
        /// </summary>
        public string AllowedAPIKeys { get; set; } = "";
    }

    /// <summary>
    /// Handler for API Key Authentication.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
    /// </remarks>
    /// <param name="options">The options monitor.</param>
    /// <param name="logger">The logger factory.</param>
    /// <param name="encoder">The URL encoder.</param>
    public class ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
    {

        /// <summary>
        /// Handles the authentication process.
        /// </summary>
        /// <returns>The authentication result.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if the request contains the API key header
            if (!this.Request.Headers.TryGetValue(Options.TokenHeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // Ensure only one API key is provided
            if (apiKeyHeaderValues.Count != 1)
            {
                return Task.FromResult(AuthenticateResult.Fail("Multiple API keys found in request"));
            }

            var apiKey = apiKeyHeaderValues[0];

            // Validate the API key
            if (Options.AllowedAPIKeys.Split(',').Contains(apiKey))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, "API Key") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));
            }
        }
    }
}
