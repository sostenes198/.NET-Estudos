using System;
using System.ComponentModel;

namespace Estudos.NotifyPropertyChanged
{
    class Program
    {
        static void Main(string[] args)
        {
            Aluno aluno = new Aluno("Macoratti", 7);
            aluno.PropertyChanged += aluno_PropertyChanged;
            aluno.Nome = "Jose Carlos Macoratti";
            aluno.AlterarNota();
            aluno.Nota = 6;
            Console.ReadLine();
        }
        private static void aluno_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"Propriedade {e.PropertyName} acabou de ser alterada...");
        }
    }
}