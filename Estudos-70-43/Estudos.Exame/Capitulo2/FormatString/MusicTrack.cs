using System;

namespace Estudos.Exame.Capitulo2.FormatString
{
    public class MusicTrack : IFormattable
    {
        public MusicTrack(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }
        
        public string Artist { get; }
        public string Title { get; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "G";

            switch (format)
            {
                case "A": return Artist;
                case "T": return Title;
                case "G":
                case "F": return ToString();
                default:
                    throw new FormatException("Format specifier was invalid");
            }
        }

        public override string ToString() => $"{Artist} {Title}";
    }
}