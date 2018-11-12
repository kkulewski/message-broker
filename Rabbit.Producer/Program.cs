using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabbit.Endpoint;

namespace Rabbit.Producer
{
    class Program
    {
        public static void Main()
        {
            Task.Run(async () => { await RunWorkers(); }).GetAwaiter().GetResult();
        }

        public static async Task RunWorkers()
        {
            Console.WriteLine("## Preparing workers...");
            var workers = new List<Worker>();
            for (var i = 0; i < 5; i++)
            {
                workers.Add(new Worker(i, FactoryProvider.Instance));
            }

            Console.WriteLine("## Starting tasks...");
            var tasks = new List<Task>();
            for (var i = 0; i < 3; i++)
            {
                foreach (var worker in workers)
                {
                    var task = new Task(worker.SendMessage);
                    task.Start();
                    tasks.Add(task);
                }
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("## All sent!");
        }
    }
}
