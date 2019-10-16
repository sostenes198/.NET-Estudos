namespace Descontructor
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public void Deconstruct(out string firstName, out string lastName)
        {
            firstName = FirstName;
            lastName = LastName;
        }
    }
}