using System;
using System.Collections.Generic;

namespace Estudos.Exame.Capitulo4.Query_And_Manipulate_Data_And_Objects_By_Using_Linq.Data
{
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

        public static (IList<MusicTrackId> MusicsTrackId, IList<ArtistId> ArtistsId) GenerateMusicTrackId()
        {
            var artistId = new List<ArtistId>();
            var musicTracks = new List<MusicTrackId>();
            var rand = new Random(1);
            var idArtistIncrementer = 0;
            var idMusicTrackIncrementer = 0;

            foreach (var artistName in _artistNames)
            {
                var newArtist = new ArtistId
                {
                    Id = idArtistIncrementer,
                    Name = artistName
                };
                artistId.Add(newArtist);
                foreach (var titleName in _titleNames)
                {
                    var newTrack = new MusicTrackId
                    {
                        Id = idMusicTrackIncrementer,
                        Title = titleName,
                        Lenght = rand.Next(20, 600),
                        ArtistId = idArtistIncrementer
                    };
                    musicTracks.Add(newTrack);
                    idMusicTrackIncrementer++;
                }

                idArtistIncrementer++;
            }

            return (musicTracks, artistId);
        }
    }
}