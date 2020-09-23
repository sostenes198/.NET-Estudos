using System;
using System.ComponentModel;

namespace ZTestes.Testes  
{  
    public enum EnumTipoClasse
    {       
        PrincipalExecucao = 'K',
        PrincipalRecurso = 'L'
    }
    
    public static class EnumTipoClasseConverter
    {

        public static T GetEnumValue<T>(this string value) where T : struct, Enum, IConvertible
        {
            return (T) Enum.ToObject(GetEnumType<T>(), value[0]) ;
        }
        private static Type GetEnumType<T>() where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new InvalidEnumArgumentException("EnumerationTypeError");
            return enumType;
        }
    }
    class Program  
    {  
        static void Main(string[] args)  
        {  
            Console.WriteLine("K"[0]);
        }  
    }
}  