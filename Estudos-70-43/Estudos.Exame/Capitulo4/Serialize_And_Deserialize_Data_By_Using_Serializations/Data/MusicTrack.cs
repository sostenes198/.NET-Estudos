using System;

namespace Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations.Data
{
    [Serializable]
    public class MusicTrack
    {
        public Artist Artist { get; set; }
        public string Title { get; set; }
        public int Lenght { get; set; }
    }
}