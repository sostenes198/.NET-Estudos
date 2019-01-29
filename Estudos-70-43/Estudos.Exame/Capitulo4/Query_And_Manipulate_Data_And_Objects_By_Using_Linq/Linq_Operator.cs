using System;
using System.Linq;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq
{
    public class Linq_Operator
    {
        public static void Test()
        {
            var musicTracks = MusicGenerator.GenerateMusicTrack();
            var selectedTracks =
                (from track in musicTracks
                    where track.Artist.Name == "Rob Miles"
                    select track).ToList();
            foreach (var track in selectedTracks)
            {
                Console.WriteLine($"Artist: {track.Artist.Name} Titile: {track.Title}");
            }
        }
    }
}