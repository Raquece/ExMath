using System.Runtime.Serialization.Formatters.Binary;

namespace ExMath.Extensions
{
    public static class Cloning
    {
        public static T DeepClone<T>(this T obj)
        {
            using var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // Type or member is obsolete (not dangerous since input is only program supplied objects)
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
        }
    }
}
