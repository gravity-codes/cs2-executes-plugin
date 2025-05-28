namespace Executes.Models
{
    // FIFO Queue
    public class ExecutesQueue<T> where T : class
    {
        private readonly List<T> Queue = new();

        public ExecutesQueue() { }

        public void Enqueue(T item)
        {
            Queue.Add(item);
        }

        public void EnqueuePriority(T item)
        {
            Queue.Insert(0, item);
        }

        public T GetNext()
        {
            return Queue.First();
        }

        public bool Drop(T item)
        {
            if (Queue.Contains(item))
            {
                Queue.Remove(item);
                return true;
            }

            return false;
        }

        public bool IsEmpty()
        {
            return Queue.Count == 0;
        }
    }
}