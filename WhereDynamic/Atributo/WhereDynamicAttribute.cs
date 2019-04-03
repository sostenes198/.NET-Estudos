using System;

namespace WhereDynamic.Atributo
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WhereDynamicAttribute : Attribute
    {
        public string NomeCampo { get; private set; }

        public WhereDynamicAttribute(string nomeCampo)
        {
            NomeCampo = nomeCampo;
        }
    }
}
