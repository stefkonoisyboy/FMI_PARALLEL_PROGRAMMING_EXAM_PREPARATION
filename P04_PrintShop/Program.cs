namespace P04_PrintShop;

class Program
{
    private static SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);
    private static CancellationTokenSource cts = new CancellationTokenSource();
    
    static void Main(string[] args)
    {
        Console.WriteLine("Print shop opened!");
        cts.CancelAfter(TimeSpan.FromSeconds(30));

        List<Thread> clients = new List<Thread>();

        for (int i = 0; i < 10; i++)
        {
            Thread client = new Thread(Client);
            clients.Add(client);
        }

        for (int i = 0; i < 10; i++)
        {
            clients[i].Start(i + 1);
        }

        foreach (var client in clients)
        {
            client.Join();
        }

        Console.WriteLine("Print shop closed!");
    }

    static void Client(object tag)
    {
        Console.WriteLine($"Client {tag} wants to borrow a machine.");

        try
        {
            semaphore.Wait(cts.Token);
            Console.WriteLine($"Client {tag} is using the machine.");

            Thread.Sleep(5000);

            Console.WriteLine($"Client {tag} finished using the machine.");
            semaphore.Release();
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Print shop closed while client {tag} was waiting: {ex.Message}");
        }
    }
}