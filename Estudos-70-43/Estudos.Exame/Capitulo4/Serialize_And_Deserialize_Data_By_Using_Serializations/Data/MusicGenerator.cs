using System;
using System.Collections.Generic;

namespace Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations.Data
{
    [Serializable]
    public class MusicGenerator
    {
        private static string[] _artistNames = {"Rob Miles", "Fred Bloggs", "The Bloggs Singers", "Immy Brown"};
        private static string[] _titleNames = {"My Way", "Your Way", "Hish Way", "Her Way", "Milky Way"};

        public static IList<MusicTrack> GenerateMusicTrack()
        {
            var musicTracks = new List<MusicTrack>();
            var rand = new Random(1);
            foreach (var artistName in _artistNames)
            {
                foreach (var titleName in _titleNames)
                {
                    var newTrack = new MusicTrack
                    {
                        Artist = new Artist {Name = artistName},
                        Title = titleName,
                        Lenght = rand.Next(20, 600)
                    };
                    musicTracks.Add(newTrack);
                }
            }

            return musicTracks;
        }
    }
}