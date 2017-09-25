using System;

namespace SearchEngine.Infrastructure.Query
{
    public interface ITypeToIndexMapperManager
    {
        ITypeToIndexMapper GetMapper(Type type);
        ITypeToIndexMapperManager RegisterMapper(Type type, ITypeToIndexMapper mapper);
    }
}