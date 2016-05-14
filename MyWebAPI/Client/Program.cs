using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            long time1, time2 = 0;

            var task = TestAsync();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var model2 = Test();
            watch.Stop();
            time2 = watch.ElapsedMilliseconds;

            watch = System.Diagnostics.Stopwatch.StartNew();
            var model1 = task.Result;
            watch.Stop();
            time1 = watch.ElapsedMilliseconds;
        }

        private static async Task<List<User>> TestAsync()
        {
            List<User> model = null;

            var client = new HttpClient();
            await client.GetAsync("http://jsonplaceholder.typicode.com/users")
                .ContinueWith(async t =>
                {
                    var response = await t;
                    model = await response.Content.ReadAsAsync<List<User>>();
                });

            return model;
        }

        private static List<User> Test()
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://jsonplaceholder.typicode.com/users").Result;
            var model = response.Content.ReadAsAsync<List<User>>().Result;

            return model;
        }
    }
}
