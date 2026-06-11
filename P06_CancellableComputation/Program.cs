namespace P06_CancellableComputation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task task = Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                for (int i = 0; i < 1000; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cleaning up...");
                        token.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(10);
                }
            }, token);

            Console.WriteLine("Press 'c' to cancel.");

            if (Console.ReadKey().KeyChar == 'c')
            {
                cts.Cancel();
            }

            try
            {
                await task;
                Console.WriteLine("Ran to completion.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("The task was cancelled.");
            }

            Console.WriteLine($"Status: {task.Status}");
        }
    }
}
