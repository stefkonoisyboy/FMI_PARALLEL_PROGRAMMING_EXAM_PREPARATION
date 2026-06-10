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
        int wanted = Random.Shared.Next(1, 5);
        
        lock (fishLock)
        {
            while (fish < wanted)
            {
                Monitor.Wait(fishLock);
            }
            
            fish -= wanted;
        }
    }

    static void Loader()
    {
        while (marketOpen)
        {
            Thread.Sleep(50);
            
            lock (fishLock)
            {
                if (fish < Capacity)
                {
                    fish++;
                    Monitor.PulseAll(fishLock);
                    Console.WriteLine($"Loaded 1 fish, now {fish}.");
                }
            }
        }
    }
}