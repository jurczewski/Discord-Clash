using DiscordClash.Application.Endpoints;
using DiscordClash.Application.Queries;
using DiscordClash.Application.UseCases.Recommender.Model;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable RedundantArgumentDefaultValue

namespace DiscordClash.Application.UseCases.Recommender
{
    public class TrainModelAndSaveItUseCase
    {
        private readonly ILogger<TrainModelAndSaveItUseCase> _logger;
        private readonly IDiscordClashApi _api;

        public TrainModelAndSaveItUseCase(ILogger<TrainModelAndSaveItUseCase> logger, IDiscordClashApi api)
        {
            _logger = logger;
            _api = api;
        }

        public async Task Execute()
        {
            var mlContext = new MLContext();
            var (trainingDataView, testDataView) = await LoadDataAsync(mlContext);
            var model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
            SaveModel(mlContext, trainingDataView.Schema, model);
        }

        private async Task<(IDataView training, IDataView test)> LoadDataAsync(MLContext mlContext)
        {
            var choices = await _api.GetAllChoices();
            var eventRatings = Map(choices).ToList();

            var data = mlContext.Data.LoadFromEnumerable(eventRatings);
            _logger.LogInformation("Loaded {@c} documents.", eventRatings.Count);

            var trainTestData = mlContext.Data.TrainTestSplit(data, 0.2);
            var trainingDataView = trainTestData.TrainSet;
            var testDataView = trainTestData.TestSet;
            _logger.LogInformation("Train/Test data was loaded.");

            return (trainingDataView, testDataView);
        }

        private ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey("userIdEncoded", "UserId")
                .Append(mlContext.Transforms.Conversion.MapValueToKey("eventIdEncoded", "EventId"));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "eventIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            _logger.LogInformation("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        private void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            _logger.LogInformation("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, "Label", "Score");
            _logger.LogInformation("Root Mean Squared Error : " + metrics.RootMeanSquaredError);
            _logger.LogInformation("RSquared: " + metrics.RSquared);
        }

        private void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "EventRecommenderModel.zip");

            _logger.LogInformation("=============== Saving the model to a file ===============");
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }

        private static IEnumerable<EventRating> Map(IEnumerable<ChoiceDto> src) //todo: move to profile
        {
            return src.Select(c => new EventRating
            {
                EventId = c.EventId.GetHashCode(),
                UserId = c.UserId.GetHashCode(),
                Label = c.Label
            });
        }
    }
}
