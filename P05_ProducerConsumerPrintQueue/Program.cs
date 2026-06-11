namespace P05_ProducerConsumerPrintQueue
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const int producerCount = 10;
            const int consumerCount = 2;
            const int queueCapacity = 25;
            const int publishTimeoutMs = 150;

            var dispatcher = new TaskDispatcher(queueCapacity, publishTimeoutMs);
            var cancelSource = new CancellationTokenSource();

            Console.WriteLine("Task Dispatcher running. Press enter to stop");

            var producerTasks = Enumerable
                .Range(1, producerCount)
                .Select(id => dispatcher.EnqueueTaskAsync(id, cancelSource.Token));

            var consumerTasks = Enumerable
                .Range(1, consumerCount)
                .Select(id => dispatcher.ProcessTasksAsync(id, cancelSource.Token));

            var allTasks = producerTasks.Concat(consumerTasks).ToArray();

            Console.ReadLine();
            cancelSource.Cancel();

            Console.WriteLine("Stopping, please wait...");
            await Task.WhenAll(allTasks);
        }
    }
}
