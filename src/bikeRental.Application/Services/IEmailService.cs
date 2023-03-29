using bikeRental.Application.Common.Email;

namespace bikeRental.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
