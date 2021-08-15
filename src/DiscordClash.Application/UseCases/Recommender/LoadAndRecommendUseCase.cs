using DiscordClash.Application.Mappings;
using DiscordClash.Application.Queries;
using DiscordClash.Application.UseCases.Recommender.Model;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using System;

namespace DiscordClash.Application.UseCases.Recommender
{
    public class LoadAndRecommendUseCase
    {
        private readonly ILogger<TrainModelAndSaveItUseCase> _logger;

        public LoadAndRecommendUseCase(ILogger<TrainModelAndSaveItUseCase> logger)
        {
            _logger = logger;
        }

        public void Execute(ChoiceDto choice)
        {
            var testInput = choice.Map();

            var mlContext = new MLContext();
            var trainedModel = mlContext.Model.Load(@"Data\EventRecommenderModel.zip", out var model);
            UseModelForSinglePrediction(mlContext, trainedModel, testInput);
        }

        private void UseModelForSinglePrediction(MLContext mlContext, ITransformer trainedModel, EventRating testInput)
        {
            _logger.LogInformation("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<EventRating, EventRatingPrediction>(trainedModel);


            var eventRatingPrediction = predictionEngine.Predict(testInput);
            _logger.LogInformation("Rating prediction score: {@score}", eventRatingPrediction.Score);
            if (Math.Round(eventRatingPrediction.Score, 1) > 3.5)
            {
                _logger.LogInformation("Event " + testInput.EventId + " is recommended for user " + testInput.UserId);
            }
            else
            {
                _logger.LogInformation("Event " + testInput.EventId + " is not recommended for user " + testInput.UserId);
            }
        }

    }
}
