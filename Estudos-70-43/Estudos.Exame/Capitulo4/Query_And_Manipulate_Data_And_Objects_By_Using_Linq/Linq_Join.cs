using System;
using System.Linq;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq
{
    public class Linq_Join
    {
        public static void Test()
        {
            var (musicTracksId, artistsId) = MusicGenerator.GenerateMusicTrackId();
            var trackDetails = (
                from artist in artistsId
                where artist.Name == "Rob Miles"
                join track in musicTracksId on artist.Id equals track.ArtistId
                select new TrackDetails
                {
                    ArtistName = artist.Name,
                    Title = track.Title
                }
            );
            foreach (var track in trackDetails)
            {
                Console.WriteLine($"Artist: {track.ArtistName} Titile: {track.Title}");
            }
        }
    }
}