using System;
using System.Text;

namespace Wrapper.Proxy
{
    public class SuperDude : IDude
    {
        private readonly IDude _dude;

        public SuperDude(IDude dude)
        {
            _dude = dude;
        }

        public string Name
        {
            get => $"Super {_dude.Name}";
            set => _dude.Name = value;
        }

        public void GotShot(string typeOfGun)
        {
            var result = new StringBuilder();
            result.Append(Name).Append(" got shot by a ").Append(typeOfGun);
            result.Append(" gun but it bounced off!!  \nYou can't hurt ").Append(Name);
            result.Append("\n\n");
            Console.WriteLine(result.ToString());
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Name).Append(" can't get hurt!");
            result.Append(" (").Append(Name).Append(" is a super-hero proxy, you know).\n");
            return result.ToString();
        }
    }
}