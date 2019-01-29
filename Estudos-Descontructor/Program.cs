using System;

namespace Descontructor
{
    internal class Program
    {
        private static void Main()
        {
            var user = GetUser();
            Console.WriteLine($"Name: {user.name}, ID: {user.id}");

            var (userId, userName) = GetUser();
            Console.WriteLine($"Name: {userName}, ID: {userId}");
        }

        private static (int id, string name) GetUser()
        {
            return (100, "Soso");
        }
    }
}