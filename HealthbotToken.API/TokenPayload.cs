namespace HealthbotToken.API
{
    internal record TokenPayload
    {
        public string TenantName { get; set; }

        public string JWTSecret { get; set; }
    }
}