using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Contexts;
using Kernel.Serialisation;

namespace Data.Importing.Helpers
{
    internal class StageResultHelper
    {
        private static readonly ConcurrentDictionary<Type, Func<ISerializer, object, object>> _delCache = new ConcurrentDictionary<Type, Func<ISerializer, object, object>>();

        public static Func<ISerializer, object, object> GetDeserialiserDelegate(Type content)
        {
            var del = StageResultHelper._delCache.GetOrAdd(content, k => StageResultHelper.BuildConstructorDelegate(k));
            return del;
        }

        private static Func<ISerializer, object, object> BuildConstructorDelegate(Type type)
        {
            var minfo = typeof(ISerializer).GetMethods()
                .SingleOrDefault(c => c.IsGenericMethod && c.Name == "Deserialize");
            if (minfo == null)
                throw new MissingMethodException("Deserialize");

            minfo = minfo.MakeGenericMethod(type);
            var processorPar = Expression.Parameter(typeof(ISerializer));
            var contentPar = Expression.Parameter(typeof(object));
            var contentParConvert = Expression.Convert(contentPar, typeof(string));
            
            var callExp = Expression.Call(processorPar, minfo,contentParConvert);
            var lambda = Expression.Lambda<Func<ISerializer, object, object>>(callExp, processorPar, contentPar);
            var del = lambda.Compile();
            return del;
        }
    }
}