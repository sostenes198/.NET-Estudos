using System;

namespace WhereDynamic.Atributo
{
    [AttributeUsage(AttributeTargets.Property)]
    public class WhereDynamicAttribute : Attribute
    {
        public WhereDynamicAttribute(string nomeCampo)
        {
            NomeCampo = nomeCampo;
        }

        public string NomeCampo { get; }
    }
}