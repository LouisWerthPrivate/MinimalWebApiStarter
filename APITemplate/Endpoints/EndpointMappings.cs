﻿using APITemplate.EndpointFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APITemplate.Endpoints
{
    public static class EndpointMappings
    {
        public static WebApplication MapEndpoints(this WebApplication app)
        {
            app.MapGet("/test", (ILogger<Program> logger) =>
            {
                logger.LogInformation("Health check endpoint called");
                return Results.Ok(new { Environment = app.Environment.EnvironmentName });
            })
            .WithName("Test")
            .WithDescription("Gets the health of the API")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError, "application/problem+json")
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable, "application/problem+json")
            .ProducesProblem(StatusCodes.Status429TooManyRequests, "application/problem+json")
            .ProducesProblem(StatusCodes.Status401Unauthorized, "application/problem+json")
            .ProducesProblem(StatusCodes.Status403Forbidden, "application/problem+json")
            .WithTags("Test")
            .AddEndpointFilter<AuditLoggingFilter>()
            .RequireAuthorization(policyNames: ["Bearer", "ApiKey"]);

            app.MapPost("/login", (ILogger<Program> logger, IConfiguration config) =>
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "user_id"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("ApiSecurityOptions:JwtBeareer:SymmetricSecurityKey", "bXlzdXBlcnNlY3JldGtleQ==bXlzdXBlcnNlY3JldGtleQ=="))); 
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: config.GetValue<string>("ApiSecurityOptions:JwtBeareer:Authority", "FallBackAuthority"),
                    audience: config.GetValue<string>("ApiSecurityOptions:JwtBeareer:Audience", "FallBackAudience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                logger.LogInformation("Login endpoint called");
                return Results.Ok(new { token = tokenString });
            }).WithName("Login")
            .WithDescription("Allows you to log in and get a token")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError, "application/problem+json")
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable, "application/problem+json")
            .ProducesProblem(StatusCodes.Status429TooManyRequests, "application/problem+json")
            .ProducesProblem(StatusCodes.Status401Unauthorized, "application/problem+json")
            .ProducesProblem(StatusCodes.Status403Forbidden, "application/problem+json")
            .WithTags("Authentication")
            .AddEndpointFilter<AuditLoggingFilter>()
            .AllowAnonymous();

            return app;
        }
    }
}