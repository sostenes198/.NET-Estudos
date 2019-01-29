using System.Linq;
using Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq
{
    public class Linq_Complex
    {
        public void Test()
        {
            var (musicTracksId, artistsId) = MusicGenerator.GenerateMusicTrackId();
            var artistSummary = musicTracksId
                .Join(artistsId, track => track.ArtistId, artist => artist.Id,
                    (track, artist) => new {track = track, artist = artist})
                .GroupBy(temp0 => temp0.artist.Name,
                    temp0 => temp0.track)
                .Select(artistTrackSummary => new
                {
                    Id = artistTrackSummary.Key,
                    Lenght = artistTrackSummary.Sum(lnq => lnq.Lenght)
                })
                .ToList();
        }
    }
}