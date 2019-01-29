using System.ComponentModel;
using System.Runtime.CompilerServices;
using Estudos.NotifyPropertyChanged.Annotations;

namespace Estudos.NotifyPropertyChanged
{
    public class Aluno : INotifyPropertyChanged
    {
        private string nome;
        public string Nome
        {
            get { return nome; }
            set
            {
                if (nome != value)
                {
                    nome = value;
                    OnPropertyChanged();
                }
            }
        }
        private int nota;
        public int Nota
        {
            get { return nota; }
            set
            {
                if (nota != value)
                {
                    nota = value;
                    OnPropertyChanged();
                }
            }
        }
        public Aluno(string nome, int nota)
        {
            Nome = nome;
            Nota = nota;
        }
        public void AlterarNota()
        {
            Nota++;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}