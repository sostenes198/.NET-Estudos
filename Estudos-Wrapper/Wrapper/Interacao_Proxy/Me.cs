using System;

namespace Wrapper.Interacao_Proxy
{
    public class Me : IInteractor
    {
        private string m_LookingAt;

        public string ChangeTo { get; set; }

        public void Percieve(string percievedThing)
        {
            m_LookingAt = percievedThing;
        }

        public void Change(ref string perceivedThing)
        {
            Console.WriteLine("I'm changing " + perceivedThing + " to a " + ChangeTo);
            perceivedThing = m_LookingAt = ChangeTo;
        }

        public override string ToString()
        {
            return "I'm looking at a " + m_LookingAt;
        }
    }
}