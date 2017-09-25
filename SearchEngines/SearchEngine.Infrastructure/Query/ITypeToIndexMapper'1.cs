using System;
using System.Collections.Generic;

namespace SearchEngine.Infrastructure.Query
{
    public interface ITypeToIndexMapper<T, TIndex> : ITypeToIndexMapper
    {
        IEnumerable<FieldDescriptor> GetFieldsContext();
        Type GetMappedIndexType();
    }
}