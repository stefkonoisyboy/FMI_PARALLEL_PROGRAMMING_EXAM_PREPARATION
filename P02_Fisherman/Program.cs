namespace P02_Fisherman;

class Program
{
    const int Capacity = 30;
    static int fish = Capacity;
    static object fishLock = new object();
    static volatile bool marketOpen = true;
    
    static void Main(string[] args)
    {
        Thread loader = new Thread(Loader);
        loader.Start();

        List<Thread> clients = new List<Thread>();
        
        for (int i = 0; i < 20; i++)
        {
            Thread t = new Thread(Client);
            clients.Add(t);
            t.Start(i);
            Thread.Sleep(10);
        }

        foreach (Thread t in clients)
        {
            t.Join();
        }

        marketOpen = false;
        loader.Join();
        
        Console.WriteLine($"Market closed with {fish} fish.");
    }
    
    static void Client(object tag)
    {
        int wanted = Random.Shared.Next(1, 8);
        
        lock (fishLock)
        {
            while (fish < wanted)
            {
                Console.WriteLine($"Client {tag} wants {wanted} fish, but only {fish} available. Waiting...");
                Monitor.Wait(fishLock);
            }
            
            fish -= wanted;
            Console.WriteLine($"Client {tag} bought {wanted} fish, now {fish}.");
        }
    }

    static void Loader()
    {
        while (marketOpen)
        {
            Thread.Sleep(50);

            int loaded = Random.Shared.Next(1, 5);

            lock (fishLock)
            {
                if (fish + loaded < Capacity)
                {
                    fish += loaded;
                    Monitor.PulseAll(fishLock);
                    Console.WriteLine($"Loaded {loaded} fish, now {fish}.");
                }
                else
                {
                    Console.WriteLine($"Could not load {loaded} fish, market full with {fish}.");
                }
            }
        }
    }
}