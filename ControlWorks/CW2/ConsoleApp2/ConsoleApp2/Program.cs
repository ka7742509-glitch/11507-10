namespace CW2
{
    class Program
    {
        static void Main()
        {
            var testObjects = new List<object>();
            for (int i = 0; i < 1000; i++)
            {
                if (i % 3 == 0)
                    testObjects.Add(new UserWithSensitive());
                else
                    testObjects.Add(new UserWithoutSensitive());
            }
    
            var processor = new ParallelProcessor();
            processor.Process(testObjects);
        }
    }
    
    class UserWithSensitive
    {
        [Sensitive]
        public string Password { get; set; } = "secret";
        public string Name { get; set; } = "User";
    }
    
    class UserWithoutSensitive
    {
        public string Name { get; set; } = "User";
    }
}
