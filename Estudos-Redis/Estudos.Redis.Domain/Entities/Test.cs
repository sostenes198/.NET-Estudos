using System;

namespace Estudos.Redis.Domain.Entities
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int Age => DateTime.Now.Date.Year - Birthday.Date.Year;
    }
}