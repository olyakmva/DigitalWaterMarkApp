using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalWaterMarkApp
{
    /**
        Класс элемента водяного знака
    */
    public class WaterMarkItem : IComparable<WaterMarkItem>
    {
        private int wmValue;
        private int rowIdx;
        private int colIdx;

        public int WMValue {
            get => wmValue;
            set => wmValue = value;
        }

        public int RowIdx {
            get => rowIdx;
            set => rowIdx = value;
        }

        public int ColIdx {
            get => colIdx;
            set => colIdx = value;
        }

        public WaterMarkItem(
            byte Value,
            int RowIdx,
            int ColIdx
        ) {
            this.WMValue = Value;
            this.RowIdx = RowIdx;
            this.ColIdx = ColIdx;
        }

        public int CompareTo(WaterMarkItem? other)
        {
            if (other == null) {
                throw new ArgumentNullException(null, "WaterMarkItem not comparing with null");
            }

            if (this.ColIdx == other.ColIdx) {
                return this.RowIdx.CompareTo(other.RowIdx);
            }

            return this.ColIdx.CompareTo(other.ColIdx);
        }

        public override string ToString()
        {
            return String.Format("({0}:{1} - {2})", this.RowIdx, this.ColIdx, this.WMValue);
        }
    }
}