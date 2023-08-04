using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Order;

namespace jwtauth.api.Config
{
    [MemoryDiagnoser]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
	public class BenchMarks
	{
		[GlobalSetup]
		public void GlobalSetUp()
		{
            //Write your initialization code here
        }

		[Benchmark(Baseline = true)]
		public void MyFirstBenchmarkMethod()
		{
            //Write your code here  
        }

        [Benchmark]
        public void MySecondBenchmarkMethod()
        {
            //Write your code here
        }
    }
}

