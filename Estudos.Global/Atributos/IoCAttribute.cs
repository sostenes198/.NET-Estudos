using Estudos.Global.Enuns;
using System;

namespace Estudos.Global.Atributos
{
    /// <summary>
    /// LifeStyle Default Transient
    /// </summary>
    public class IoCAttribute : LifeStyleAttribute
    {
        public IoCAttribute()
            : base(LifeStyleIoCEnum.Transient)
        { }

        public IoCAttribute(LifeStyleIoCEnum lifeStyleIoCEnum)
            :base(lifeStyleIoCEnum)
        {
        }
    }
}
