using AutoMapper;
using Estudos.AutoMapper.Base;
using System.Linq;
using System.Reflection;

namespace Estudos.AutoMapper
{
    public class ConfigMapper
    {
        public static void RegistrarMapas()
        {
            var all =
       Assembly
          .GetEntryAssembly()
          .GetReferencedAssemblies()
          .Select(Assembly.Load)
          .SelectMany(x => x.DefinedTypes)
          .Where(type => typeof(IProfile).GetTypeInfo().IsAssignableFrom(type.AsType()));

            foreach (var ti in all)
            {
                var t = ti.AsType();
                if (t.Equals(typeof(IProfile)))
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfiles(t); // Initialise each Profile classe
                    });
                }
            }
        }
    }
}
