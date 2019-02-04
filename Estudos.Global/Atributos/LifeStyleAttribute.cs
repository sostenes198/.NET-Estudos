using Estudos.Global.Enuns;
using System;

namespace Estudos.Global.Atributos
{
    public class LifeStyleAttribute : Attribute
    {
        public LifeStyleIoCEnum LifeStyleIoCEnum { get; private set; }

        public LifeStyleAttribute()
            : this(LifeStyleIoCEnum.Transient)
        { }

        public LifeStyleAttribute(LifeStyleIoCEnum lifeStyleIoCEnum)
        {
            LifeStyleIoCEnum = lifeStyleIoCEnum;
        }
    }
}
