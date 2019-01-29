namespace Wrapper.Interacao_Proxy
{
    public class House
    {
        private string m_thing = "vase";

        public void LookAtThing(IInteractor person)
        {
            person.Percieve(m_thing);
        }

        public void ChangeThing(IInteractor person)
        {
            person.Change(ref m_thing);
        }

        public override string ToString()
        {
            return "This house has a " + m_thing;
        }
    }
}