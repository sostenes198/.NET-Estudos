using System;
using System.Linq;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq
{
    public class Linq_Agregate
    {
        public static void Test()
        {
            var (musicTracksId, artistsId) = MusicGenerator.GenerateMusicTrackId();
            var artistSummary = (
                from track in musicTracksId
                join artist in artistsId on track.ArtistId equals artist.Id
                group track by artist.Name
                into artistTrackSummary
                select new
                {
                    Id = artistTrackSummary.Key,
                    Lenght = artistTrackSummary.Sum(x => x.Lenght)
                }
            );
            foreach (var item in artistSummary)
            {
                Console.WriteLine($"Artist: {item.Id} Lenght: {item.Lenght}");
            }
        }
    }
}