namespace P03_RandomNumbers;

class Program
{
    static async Task Main(string[] args)
    {
        Task<int> t1 = Task.Run(() => Random.Shared.Next(1, 11));
        Task<int> t2 = Task.Run(() => Random.Shared.Next(1, 11));
        Task<int> t3 = Task.Run(() => Random.Shared.Next(1, 11));
        Task<int> t4 = Task.Run(() => Random.Shared.Next(1, 11));
        Task<int> t5 = Task.Run(() => Random.Shared.Next(1, 11));

        try
        {
            var result = await Task.WhenAll(t1, t2, t3, t4, t5);
            Console.WriteLine($"Final sum: {result.Sum()}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }
}