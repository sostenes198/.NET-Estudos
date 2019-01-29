using System;

namespace Estudos.Exame.Capitulo2.SystemReflection
{
    public class SystemReflectionStudy
    {
        public static void GetMemberOfPerson()
        {
            var person = new Person();
            var type = person.GetType();
            foreach (var member in type.GetMembers())
            {
                Console.WriteLine(member.ToString());
            }
        }

        public static void SetNamePerson()
        {
            var person = new Person();
            var type = person.GetType();
            var setMethod = type.GetMethod("set_Name");
            setMethod.Invoke(person, new[] {"Soso"});
            Console.WriteLine(person.Name);
        }
    }
}