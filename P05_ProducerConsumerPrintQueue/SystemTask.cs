namespace P05_ProducerConsumerPrintQueue
{
    public class SystemTask
    {
        public Guid Id { get; set; }

        public int ProducerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public TaskType TaskType { get; set; }

        public override string ToString() =>
            $"{TaskType} (Producer={ProducerId}, CreatedAt={CreatedAt:HH:mm:ss.fff})";
    }
}
