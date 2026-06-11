namespace P08_VisitorCounter
{
    internal class Program
    {
        static long counter = 0;
        static long maxDuration = 0;

        static object counterLock = new object();
        static object maxDurationLock = new object();

        static void Main(string[] args)
        {
            var threads = Enumerable.Range(0, 10)
                .Select(i => new Thread(Worker))
                .ToArray();

            foreach (var t in threads)
            {
                t.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            Console.WriteLine($"Visitors: {counter}");
            Console.WriteLine($"Max duration: {maxDuration}");
        }

        static void Worker()
        {
            for (int i = 0; i < 100_000; i++)
            {
                lock (counterLock) 
                {
                    counter++;
                }

                long duration = Random.Shared.Next(1, 1000);

                lock (maxDurationLock)
                {
                    if (duration > maxDuration)
                    {
                        maxDuration = duration;
                    }
                }
            }
        }
    }
}
