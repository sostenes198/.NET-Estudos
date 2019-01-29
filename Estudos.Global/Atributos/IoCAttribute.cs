using Estudos.Global.Enuns;
using System;

namespace Estudos.Global.Atributos
{
    public class IoCAttribute : Attribute
    {
        public LifeStyleIoCEnum LifeStyleIoCEnum;

        public IoCAttribute()
            : this(LifeStyleIoCEnum.Transient)
        { }

        public IoCAttribute(LifeStyleIoCEnum lifeStyleIoCEnum)
        {
            LifeStyleIoCEnum = lifeStyleIoCEnum;
        }
    }
}
