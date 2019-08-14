using System;
using System.Text;

namespace Wrapper.Proxy
{
    public class SuperDude : IDude
    {
        public string Name
        {
            get => $"Super {_dude.Name}";
            set => _dude.Name = value;
        }

        private readonly IDude _dude;

        public SuperDude(IDude dude)
        {
            _dude = dude;
        }

        public void GotShot(string typeOfGun)
        {
            StringBuilder result = new StringBuilder();
            result.Append(Name).Append(" got shot by a ").Append(typeOfGun);
            result.Append(" gun but it bounced off!!  \nYou can't hurt ").Append(Name);
            result.Append("\n\n");
            Console.WriteLine(result.ToString());
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Name).Append(" can't get hurt!");
            result.Append(" (").Append(Name).Append(" is a super-hero proxy, you know).\n");
            return result.ToString();
        }
    }
}