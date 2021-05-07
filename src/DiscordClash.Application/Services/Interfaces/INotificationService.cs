using DiscordClash.Application.Commands;
using System.Threading.Tasks;

namespace DiscordClash.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAboutNewEvent(CreateNewEvent cmd);
    }
}
