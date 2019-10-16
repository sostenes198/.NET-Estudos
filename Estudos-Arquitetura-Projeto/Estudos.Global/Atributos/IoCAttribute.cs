using Estudos.Global.Enuns;

namespace Estudos.Global.Atributos
{
    /// <summary>
    ///     LifeStyle
    ///     Default
    ///     Transient
    /// </summary>
    public class IoCAttribute : LifeStyleAttribute
    {
        public IoCAttribute()
            : base(LifeStyleIoCEnum.Transient)
        {
        }

        public IoCAttribute(LifeStyleIoCEnum lifeStyleIoCEnum)
            : base(lifeStyleIoCEnum)
        {
        }
    }
}