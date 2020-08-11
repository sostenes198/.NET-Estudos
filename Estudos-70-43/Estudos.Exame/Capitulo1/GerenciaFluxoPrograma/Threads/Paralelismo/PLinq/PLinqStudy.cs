using System;
using System.Linq;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Paralelismo.PLinq
{
    public class PLinqStudy
    {
        static readonly PersonStudy[] people = new PersonStudy[]
        {
            new PersonStudy {Name = "Allan", City = "Hull"},
            new PersonStudy {Name = "Beryl", City = "Seattle"},
            new PersonStudy {Name = "Charles", City = "London"},
            new PersonStudy {Name = "David", City = "Seattle"},
            new PersonStudy {Name = "Eddy", City = "Paris"},
            new PersonStudy {Name = "Fred", City = "Hull"},
            new PersonStudy {Name = "Gordon", City = "Hull"},
            new PersonStudy {Name = "Gordon1", City = "Hull"},
            new PersonStudy {Name = "Gordon2", City = "Hull"},
            new PersonStudy {Name = "Gordon3", City = "Hull"},
            new PersonStudy {Name = "Gordon4", City = "Hull"},
            new PersonStudy {Name = "Gordon4", City = ""},
            new PersonStudy {Name = "Gordon4", City = ""},
            new PersonStudy {Name = "Gordon4", City = ""}
        };

        public static void InvokePlinq()
        {
            var result = from person in people.AsParallel()
                    .AsOrdered()
                where person.City == "Seattle"
                select person;

            foreach (var person in result)
            {
                Console.WriteLine(person.Name);
            }
        }

        public static void InovokePLinqForced()
        {
            var result = from person in people.AsParallel()
                    .WithDegreeOfParallelism(4)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                where person.City == "Hull"
                select person;

            foreach (var person in result)
            {
                Console.WriteLine(person.Name);
            }
        }

        public static void InovokePLinqAsSequencial()
        {
            var result =
                (from person in people.AsParallel()
                    where person.City == "Hull"
                    orderby (person.Name)
                    select person)
                .Take(4);

            foreach (var person in result)
            {
                Console.WriteLine(person.Name);
            }
        }

        public static void InvokePLinqForAll()
        {
            var result = from person in people.AsParallel()
                where person.City == "Hull"
                select person;

            result.ForAll(person => Console.WriteLine(person.Name));
        }

        public static void InvokePLinqException()
        {
            try
            {
                var result = from person in people.AsParallel()
                    where CheckCity(person.City)
                    select person;

                result.ForAll(person => Console.WriteLine(person.Name));
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"{ex.InnerExceptions.Count} exceptions");
            }
        }

        private static bool CheckCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("Ruim pa carai");

            return city == "Hull";
        }
    }
}