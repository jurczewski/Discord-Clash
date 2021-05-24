using DiscordClash.Application.Queries;
using DiscordClash.Recommender.Endpoints;
using DiscordClash.Recommender.Model;
using Figgle;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordClash.Recommender
{
    public class Program
    {
        private static async Task Main()
        {
            DisplayBanner();

            var mlContext = new MLContext();
            var (trainingDataView, testDataView) = await LoadDataAsync(mlContext);
            var model = BuildAndTrainModel(mlContext, trainingDataView);
        }

        private static async Task<(IDataView training, IDataView test)> LoadDataAsync(MLContext mlContext)
        {
            var api = RestService.For<IDiscordClashApi>("https://localhost:5001");
            var choices = await api.GetAllChoices();
            var eventRatings = Map(choices);
            var data = mlContext.Data.LoadFromEnumerable(eventRatings);

            //eventRatings.ForAll(Console.WriteLine); // todo: logger count

            var trainTestData = mlContext.Data.TrainTestSplit(data, 0.2);
            var trainingDataView = trainTestData.TrainSet;
            var testDataView = trainTestData.TestSet;

            return (trainingDataView, testDataView);
        }

        private static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
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
            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
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

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
