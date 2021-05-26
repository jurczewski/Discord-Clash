using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases.API
{
    public class GetAllChoicesUseCase
    {
        private readonly IGenericRepository<Choice> _choiceRepository;
        private readonly ILogger<GetAllChoicesUseCase> _logger;

        public GetAllChoicesUseCase(IGenericRepository<Choice> choiceRepository, ILogger<GetAllChoicesUseCase> logger)
        {
            _choiceRepository = choiceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Choice>> Execute()
        {
            var choices = await _choiceRepository.GetAll();
            var choicesList = choices.ToList();
            _logger.LogInformation("Fetched all choices. Count: {@c}.", choicesList.Count);
            return choicesList;
        }
    }
}
