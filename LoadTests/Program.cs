using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTests
{
    /// <summary>
    /// Do not assess this code. This is not how I'd do load testing in real life.
    /// This is not how I'd write code in production.
    /// </summary>
    class Program
    {
        private static HttpClient _http = new();
        private static int _executing = 0;
        private static int _executed = 0;
        private static Stopwatch _sw = new();

        static async Task Main(string[] args)
        {
            await MakeRequests(100, 500000);
            Console.WriteLine("Finished in: " + _sw.ElapsedMilliseconds / 1000d + "sec");
        }

        private static Task MakeRequests(int parallelismLevel, int requestNumber)
        {
            _sw.Restart();
            _executing = 0;
            _executed = 0;
            var tasks = new Task[parallelismLevel];
            while (parallelismLevel-- > 0)
            {
                tasks[parallelismLevel] = Task.Run(() => MakeRequests(requestNumber));
            }

            return Task.WhenAll(tasks);
        }

        private static async Task MakeRequests(int maxRequests)
        {
            maxRequests /= 2; //every loop cycle we do 2 requests
            while (Interlocked.Increment(ref _executing) <= maxRequests)
            {
                await MakeRequestByCity();
                Print();
                await MakeRequestByIp();
                Print();
            }
        }

        private static void Print()
        {
            var count = Interlocked.Increment(ref _executed);

            if (count % 1000 == 0)
            {
                var requestTime = (double)_sw.ElapsedMilliseconds / count;
                var elapsedSec = _sw.ElapsedMilliseconds / 1000d;
                var rps = count / elapsedSec;
                Console.WriteLine(count + " requests executed. Request time: " + requestTime + ". RPS: " + rps);
            }
        }

        private static Task MakeRequestByIp()
        {
            return MakeRequest("ip/location?ip=" + GenerateRandomIp());
        }

        private static Task MakeRequestByCity()
        {
            string[] cities = {"cit_Ikakelyzohaxih", "cit_Ulubimykaxypexi", "cit_Iwagahehyfowu", "cit_Isy", "city_doesNotExist", ""};
            var r = new Random();

            return MakeRequest("city/locations?city=" + cities[r.Next(0, cities.Length)]);
        }

        private static async Task MakeRequest(string url)
        {
            var result = await _http.GetAsync("http://localhost:5000/" + url);
            //Console.WriteLine(await result.Content.ReadAsStringAsync());
        }

        private static string GenerateRandomIp()
        {
            var random = new Random();
            return random.Next(0, 255) + "." + random.Next(0, 255) + "." + random.Next(0, 255) + "." + random.Next(0, 255);
        }
    }
}