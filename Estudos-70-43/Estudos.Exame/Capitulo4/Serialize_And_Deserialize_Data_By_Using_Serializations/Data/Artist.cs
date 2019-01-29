using System;
using System.Runtime.Serialization;

namespace Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations.Data
{
    [Serializable]
    public class Artist
    {
        public string Name { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Console.WriteLine("Called before the artist is serialized");
        }

        [OnSerialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            Console.WriteLine("Called after the artist is serialized");
        }

        [OnDeserializing]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            Console.WriteLine("Called before the artist is deserialized");
        }
    }
}