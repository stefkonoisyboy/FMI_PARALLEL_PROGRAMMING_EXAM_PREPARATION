using System.Collections.Concurrent;

namespace P05_ProducerConsumerPrintQueue
{
    public class TaskDispatcher
    {
        private readonly BlockingCollection<SystemTask> _taskQueue;
        private readonly int _publishTimeoutMs;

        public TaskDispatcher(int maxQueueSize, int publishTimeoutMs)
        {
            _publishTimeoutMs = publishTimeoutMs;
            _taskQueue = new BlockingCollection<SystemTask>(new ConcurrentQueue<SystemTask>(), maxQueueSize);
        }

        // Producer method
        public async Task EnqueueTaskAsync(int producerId, CancellationToken token)
        {
            var task = new SystemTask
            {
                Id = Guid.NewGuid(),
                ProducerId = producerId,
                CreatedAt = DateTime.UtcNow,
                TaskType = GetRandomTaskType()
            };

            // Simulated external operation
            await Task.Delay(Random.Shared.Next(10, 60), token);

            bool queueAdded = _taskQueue.TryAdd(task, _publishTimeoutMs, token);

            if (queueAdded)
                Console.WriteLine($"[Producer {producerId}] Task queued: {task.ToString()}");
            else
                Console.WriteLine($"[Producer {producerId}] Queue FULL - Task dropped: {task.Id}");
        }

        // Consumer method
        public async Task ProcessTasksAsync(int consumerId, CancellationToken token)
        {
            foreach (var task in _taskQueue.GetConsumingEnumerable(token))
            {
                Console.WriteLine($"--> [Consumer {consumerId}] Processing {task.TaskType} ({task.Id})");

                await Task.Delay(Random.Shared.Next(20, 80), token);

                Console.WriteLine($"--> [Consumer {consumerId}] Completed {task.Id}");
            }
        }

        private static TaskType GetRandomTaskType()
        {
            var values = Enum.GetValues<TaskType>();
            return values[Random.Shared.Next(values.Length)];
        }
    }
}
