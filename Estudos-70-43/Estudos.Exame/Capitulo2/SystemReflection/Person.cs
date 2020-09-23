namespace Estudos.Exame.Capitulo2.SystemReflection
{
    public class Person
    {
        public string Name { get; set; }
        
        public Person CreatePerson(string name) => new Person{Name = name};
    }
}