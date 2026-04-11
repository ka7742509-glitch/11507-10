//1 задание
namespace OF
{
    public static class ObjectFactory
    {
        
        public static T Create<T>() where T : new() 
        {
            return new T();
        }
    
        public static List<T> CreateList<T>(int count) where T : new() 
        {
            var list = new List<T>();
            if (count < 0)
                return list;
                
            for (int i = 0; i < count; i++)
            {
                list.Add(new T());
            }
            return list;
        }
    
        public static T CreateAndAction<T>(Action<T> initializer) where T : new() 
        {
            var obj = new T();
            initializer?.Invoke(obj);
            return obj;
        }
    }
    
    public class Tool
    {
        public string Name { get; set; }
    }

    public class Worker
    {
        public string Name { get; set; }
    }
}


