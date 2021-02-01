using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations.Data;

namespace Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations
{
    public class Binary_Serialization
    {
        public static void Test()
        {
            var musicData = MusicGenerator.GenerateMusicTrack();
            var formatter = new BinaryFormatter();
            using (FileStream outputStream = new FileStream("MusicTracks.bin", FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(outputStream, musicData);
            }

            using (FileStream inputStream = new FileStream("MusicTracks.bin", FileMode.Open, FileAccess.Read))
            {
                var inputData = (IList<MusicTrack>) formatter.Deserialize(inputStream);
            }
        }
    }
}