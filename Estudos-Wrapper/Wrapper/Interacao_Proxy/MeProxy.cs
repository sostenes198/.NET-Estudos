using System;

namespace Wrapper.Interacao_Proxy
{
    public class MeProxy : IInteractor
    {
        private readonly Me m_wrappedObject;

        public MeProxy(Me wrappedObject)
        {
            m_wrappedObject = wrappedObject;
        }

        public void Percieve(string percievedThing)
        {
            Console.WriteLine("Perception by Proxy");
            m_wrappedObject.Percieve(percievedThing);
        }

        public void Change(ref string perceivedThing)
        {
            Console.WriteLine("Change by Proxy");
            m_wrappedObject.Change(ref perceivedThing);
        }

        public override string ToString()
        {
            return "I'm an apparition.  The real me says:" + m_wrappedObject;
        }
    }
}