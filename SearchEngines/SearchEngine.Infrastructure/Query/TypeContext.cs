using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kernel.Initialisation;
using Kernel.Serialisation;

namespace SearchEngine.Infrastructure.Query
{
    public abstract class TypeContext
    {
        public virtual Type Type { get; }
        public Type ResultServiceType { get; set; }

        public abstract IEnumerable<TProjection> PtojectTo<TProjection>(IEnumerable<string> source);
    }
    public class TypeContext<T, TView, TResult> : TypeContext
    {
        private static ConcurrentDictionary<Type, Func<ISerializer, string, TView>> _delegates = new ConcurrentDictionary<Type, Func<ISerializer, string, TView>>();
        public override Type Type { get { return typeof(T); } }
        
        public Func<IEnumerable<TView>, IEnumerable<TResult>> Projection { get; set; }

        public override IEnumerable<TProjection> PtojectTo<TProjection>(IEnumerable<string> source)
        {
            try
            {
                var resolver = ApplicationConfiguration.Instance.DependencyResolver;
                var jsonDeserialiser = resolver.Resolve<ISerializer>();
                var del = TypeContext<T, TView, TResult>._delegates.GetOrAdd(typeof(TView), k =>
                {
                    var minfo = typeof(ISerializer).GetMethods()
                        .FirstOrDefault(x => x.Name == "Deserialize" && x.IsGenericMethod);
                    if (minfo == null)
                        throw new MissingMethodException("ISerializer", "Deserialize<T>(string data)");
                    minfo = minfo.MakeGenericMethod(typeof(TView));
                    var target = Expression.Parameter(typeof(ISerializer));
                    var p = Expression.Parameter(typeof(string));
                    var call = Expression.Call(target, minfo, p);
                    var d = Expression.Lambda<Func<ISerializer, string, TView>>(call, target, p)
                        .Compile();
                    return d;
                });
                var deserialised = source.Select(x => del(jsonDeserialiser, x));
                var r = this.Projection(deserialised);
                return r.Cast<TProjection>();
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
