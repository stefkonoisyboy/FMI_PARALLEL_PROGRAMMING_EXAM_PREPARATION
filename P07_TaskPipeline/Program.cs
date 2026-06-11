namespace P07_TaskPipeline
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> t1 = Task.Run(() => Random.Shared.Next(1, 11));
            Task<int> t2 = t1.ContinueWith(t => t.Result * 2);
            Task t3 = t2.ContinueWith(t => Console.WriteLine($"Result: {t.Result}"));
            
            t3.Wait();

            Task<int> t4 = Task.Run(() => Random.Shared.Next(1, 11));
            Task<int> t5 = t4.ContinueWith(t => t.Result * 2);
            Task<int> t6 = t4.ContinueWith(t => t.Result * 2);
            Task<int> t7 = t4.ContinueWith(t => t.Result * 2);

            var results = Task.WhenAll(t5, t6, t7).Result;

            Console.WriteLine($"Sum: {results.Sum()}");
        }
    }
}
