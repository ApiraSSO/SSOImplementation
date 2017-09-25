using System;

namespace SearchEngine.Infrastructure
{
    public class IndexContext
    {
        private string _indexName;

        public IndexContext(Type indexType, string indexName = null)
        {
            if (indexType == null)
                throw new ArgumentNullException("indexType");

            this.IndexType = indexType;
            this.IndexName = indexName;
        }
        public string IndexName
        {
            get
            {
                return this._indexName ?? this.IndexType.Name.ToLower();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                this._indexName = value.ToLower();
            }
        }
        public Type IndexType { get; private set; }
    }
}