using System.Threading.Tasks;
using bikeRental.Application.Common.Email;
using bikeRental.Application.Services;

namespace bikeRental.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
