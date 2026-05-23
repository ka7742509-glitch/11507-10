using System.Reflection;

namespace CW2
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveAttribute : Attribute { }

    public class AttributeFilter
    {
        public List<object> GetValidObjects(List<object> objects)
        {
            return objects.Where(obj => obj.GetType().GetProperties()
                .Any(prop => prop.GetCustomAttribute<SensitiveAttribute>() != null)).ToList();
        }
    }
}
