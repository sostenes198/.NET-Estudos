using System;
using System.Linq;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq
{
    public class Linq_Take_And_Skip
    {
        public static void Test()
        {
            int pageNo = 0;
            int pageSize = 2;
            var musicTracks = MusicGenerator.GenerateMusicTrack();
            while (true)
            {
                var trackList = (
                    from musicTrack in musicTracks.Skip(pageNo * pageSize).Take(pageSize)
                    select new TrackDetails
                    {
                        ArtistName = musicTrack.Artist.Name,
                        Title = musicTrack.Title
                    }
                ).ToList();
                if (trackList.Count == 0)
                    break;
                foreach (var track in trackList)
                {
                    Console.WriteLine($"Artist: {track.ArtistName} Titile: {track.Title}");
                }

                pageNo++;
            }
        }
    }
}