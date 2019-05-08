using System;

namespace Descontructor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var user = GetUser();
            Console.WriteLine($"Name: {user.name}, ID: {user.id}");

            (var userId, var userName)= GetUser();
            Console.WriteLine($"Name: {userName}, ID: {userId}");

            var jhon = new User()
            {
                FirstName = "Jhon",
                LastName = "Whick",
                Age = 33,
                Email = "JhonWick@Jhon.Jhon"
            };

            (string firstName, string lastName) = jhon;
        }

        static (int id, string name) GetUser()
        {
            return (100, "Soso");
        }
    }
}
