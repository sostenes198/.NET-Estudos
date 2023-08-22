using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.CreateAndMonitorPerformanceCounters
{
    public class Create_Performance_Counters
    {
        private static PerformanceCounter _totalImageCounter;
        private static PerformanceCounter _imagensPerSecondCounter;

        private enum CreationResult
        {
            CreatedCounters,
            LoadedCounters
        }

        public static void Test()
        {
            SetupPerformanceCounters();
            _totalImageCounter.Increment();
            _imagensPerSecondCounter.Increment();
        }

        private static CreationResult SetupPerformanceCounters()
        {
            var categoryName = "Image processing";
            var counterNameImagesProcessed = "% of images processed";
            var counterNameImagesPerSecond = "# images processed per second";
            if (PerformanceCounterCategory.Exists(categoryName))
            {
                // production code should use using
                _totalImageCounter = new PerformanceCounter(categoryName,
                    counterNameImagesProcessed,
                    false);
                
                // production code should use using
                _imagensPerSecondCounter = new PerformanceCounter(categoryName,
                    counterNameImagesPerSecond,
                    false);

                return CreationResult.LoadedCounters;
            }
            
            var counters = new[]
            {
                new CounterCreationData(counterNameImagesProcessed,
                    "number of images resized",
                    PerformanceCounterType.NumberOfItems64),
                new CounterCreationData(counterNameImagesPerSecond,
                    "number of images processed per second",
                    PerformanceCounterType.RateOfCountsPerSecond32)
            }; 
            
            var counterCollection = new CounterCreationDataCollection(counters);
            PerformanceCounterCategory.Create(categoryName,
                "Image processing information",
                PerformanceCounterCategoryType.SingleInstance,
                counterCollection);
            return CreationResult.CreatedCounters;
        }
    }
}