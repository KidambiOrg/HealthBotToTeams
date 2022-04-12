using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace HealthbotToken.API
{
    public class TokenFunction
    {
        private readonly ILogger _logger;

        public TokenFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TokenFunction>();
        }

        [Function("HealthBotToken")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Getting token");

            TokenPayload payload = JsonConvert.DeserializeObject<TokenPayload>(await new StreamReader(req.Body).ReadToEndAsync());

            TokenResponse token = new TokenResponse() { token = await GetTokenAsync(payload.JWTSecret, payload.TenantName).ConfigureAwait(false) };

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(token).ConfigureAwait(false);

            return response;
        }

        private async Task<string> GetTokenAsync(string healthbotJWTSecret, string healthbotTenantName)
        {
            try
            {
                if (String.IsNullOrEmpty(healthbotJWTSecret) || String.IsNullOrEmpty(healthbotTenantName))
                {
                    return null;
                }

                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(healthbotJWTSecret));
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                var header = new JwtHeader(signingCredentials);
                var payload = new JwtPayload
                {
                    { "tenantName", "" + healthbotTenantName + ""},
                    { "iat", secondsSinceEpoch},
                };

                var secToken = new JwtSecurityToken(header, payload);
                var handler = new JwtSecurityTokenHandler();

                var tokenString = handler.WriteToken(secToken);
                return tokenString;
            }
            catch (ArgumentNullException argNullEx)
            {
                throw new ArgumentNullException(argNullEx.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}