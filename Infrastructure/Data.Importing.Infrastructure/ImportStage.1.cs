using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Importing.Infrastructure
{
    public class ImportStage<TStage> : IEquatable<ImportStage<TStage>>, IComparable<ImportStage<TStage>> where TStage : struct, IConvertible
    {
        public TStage Stage { get; private set; }

        public ImportStage(TStage stage)
        {
            this.Stage = stage
;        }

        public bool Equals(ImportStage<TStage> other)
        {
            return this.Stage.Equals(other.Stage);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            var stage = obj as ImportStage<TStage>;
            if (stage == null)
                return false;
            else
                return Equals(stage);
        }

        public override int GetHashCode()
        {
            return this.Stage.GetHashCode();
        }

        public int CompareTo(ImportStage<TStage> other)
        {
            if (this.Equals(other))
                return 0;
            
            return (this.Stage.ToInt32(CultureInfo.CurrentCulture) > other.Stage.ToInt32(CultureInfo.CurrentCulture)) ? 1 : -1;
        }

        public static bool operator ==(ImportStage<TStage> stage1, ImportStage<TStage> stage2)
        {
            if (((object)stage1) == null || ((object)stage2) == null)
                return Object.Equals(stage1, stage2);

            return stage1.Equals(stage2);
        }

        public static bool operator !=(ImportStage<TStage> stage1, ImportStage<TStage> stage2)
        {
            if (((object)stage1) == null || ((object)stage2) == null)
                return !Object.Equals(stage1, stage2);

            return !(stage1.Equals(stage2));
        }
    }
}