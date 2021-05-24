using DiscordClash.Recommender.Endpoints;
using Figgle;
using Microsoft.ML;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.Internal;
using DiscordClash.Application.Queries;
using DiscordClash.Recommender.Model;

namespace DiscordClash.Recommender
{
    public class Program
    {
        private static async Task Main()
        {
            DisplayBanner();

            var mlContext = new MLContext();
            var (trainingDataView, testDataView) = await LoadDataAsync(mlContext);
        }

        private static async Task<(IDataView training, IDataView test)> LoadDataAsync(MLContext mlContext)
        {
            var api = RestService.For<IDiscordClashApi>("https://localhost:5001");
            var choices = await api.GetAllChoices();
            var eventRatings = Map(choices);
            var data = mlContext.Data.LoadFromEnumerable(eventRatings);

            eventRatings.ForAll(Console.WriteLine); // todo: logger count

            var trainTestData = mlContext.Data.TrainTestSplit(data, 0.2);
            var trainingDataView = trainTestData.TrainSet;
            var testDataView = trainTestData.TestSet;

            return (trainingDataView, testDataView);
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
