using System;
using System.Diagnostics.CodeAnalysis;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload
{
    
    [ExcludeFromCodeCoverage]
    public class ShortGuid
    {
        private readonly Guid _guid;
        private readonly string _value;

        public ShortGuid(Guid guid)
        {
            if (guid == default)
                throw new ArgumentNullException(nameof(guid));

            _guid = guid;
            
            _value = Convert.ToBase64String(guid.ToByteArray())
                .Substring(0, 22)
                .Replace("/", "_")
                .Replace("+", "-");
        }

        /// <summary>Get the short GUID as a string.</summary>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>Get the Guid object from which the short GUID was created.</summary>
        public Guid ToGuid()
        {
            return _guid;
        }

        /// <summary>Get a short GUID as a Guid object.</summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        public static ShortGuid Parse(string shortGuid)
        {
            if (shortGuid == null)
                throw new ArgumentNullException(nameof(shortGuid));
            
            if (shortGuid.Length != 22)
                throw new FormatException("Input string was not in a correct format.");

            return new ShortGuid(new Guid(Convert.FromBase64String
                (shortGuid.Replace("_", "/").Replace("-", "+") + "==")));
        }

        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        public static implicit operator String(ShortGuid guid)
        {
            return guid.ToString();
        }

        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid._guid;
        }
    }
}