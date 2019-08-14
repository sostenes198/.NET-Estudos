using System;
using System.Text;

namespace Wrapper.Proxy
{
    public class NormalDude : IDude
    {
        private string m_Name = string.Empty;

        private bool m_IsHurt = false;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public void GotShot(string typeOfGun)
        {
            m_IsHurt = true;

            Console.WriteLine(m_Name + " got shot by a " + typeOfGun + " gun.");
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(m_Name);
            if (m_IsHurt)
                result.Append(" is hurt");
            else
                result.Append(" is as healthy as a clam");

            return result.ToString();
        }
    }
}