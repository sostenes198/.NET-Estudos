using System;
using System.Text;

namespace Wrapper.Proxy
{
    public class NormalDude : IDude
    {
        private bool m_IsHurt;

        public string Name { get; set; } = string.Empty;

        public void GotShot(string typeOfGun)
        {
            m_IsHurt = true;

            Console.WriteLine(Name + " got shot by a " + typeOfGun + " gun.");
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Name);
            if (m_IsHurt)
                result.Append(" is hurt");
            else
                result.Append(" is as healthy as a clam");

            return result.ToString();
        }
    }
}