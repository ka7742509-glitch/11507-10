namespace CW2
{
    public class ParallelProcessor
    {
        private List<List<object>> SplitIntoChunks(List<object> objects, int chunkCount)
        {
            var chunks = new List<List<object>>();
            int chunkSize = (int)Math.Ceiling(objects.Count / (double)chunkCount);

            for (int i = 0; i < chunkCount; i++)
            {
                chunks.Add(objects.Skip(i * chunkSize).Take(chunkSize).ToList());
                
            }
            return chunks;
        }
        
        private readonly AttributeFilter _filter = new AttributeFilter();
        
        public void Process(List<object> objects)
        {
            var chunks = SplitIntoChunks(objects, 4);
            Parallel.For(0, chunks.Count, i =>
            {
                var chunk = chunks[i];
                var validObjects = _filter.GetValidObjects(chunk);
                Console.WriteLine(
                    $"ThreadID: {Task.CurrentId ?? Environment.CurrentManagedThreadId}, Processed: {chunk.Count}, Valid: {validObjects.Count}");
            });
        }
    }
}

