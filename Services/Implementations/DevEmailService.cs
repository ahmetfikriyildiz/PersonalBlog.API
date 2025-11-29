using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<DevEmailService> _logger;

        public DevEmailService(ILogger<DevEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogWarning("**************** DEVELOPMENT EMAIL ****************");
            _logger.LogWarning($"To: {to}");
            _logger.LogWarning($"Subject: {subject}");
            _logger.LogWarning("Body:");
            _logger.LogWarning(body);
            _logger.LogWarning("***************************************************");
            
            return Task.CompletedTask;
        }
    }
}

