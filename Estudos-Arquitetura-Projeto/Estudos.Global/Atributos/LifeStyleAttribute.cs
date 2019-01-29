using System;
using Estudos.Global.Enuns;

namespace Estudos.Global.Atributos
{
    public class LifeStyleAttribute : Attribute
    {
        public LifeStyleAttribute()
            : this(LifeStyleIoCEnum.Transient)
        {
        }

        public LifeStyleAttribute(LifeStyleIoCEnum lifeStyleIoCEnum)
        {
            LifeStyleIoCEnum = lifeStyleIoCEnum;
        }

        public LifeStyleIoCEnum LifeStyleIoCEnum { get; }
    }
}