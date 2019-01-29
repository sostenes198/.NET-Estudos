using System.ComponentModel.DataAnnotations;

namespace Estudos.Global.Enuns
{
    public enum LifeStyleIoCEnum
    {
        [Display(Name = "Transient")] Transient = 1,

        [Display(Name = "Scoped")] Scoped = 2,

        [Display(Name = "Singleton")] Singleton = 3
    }
}