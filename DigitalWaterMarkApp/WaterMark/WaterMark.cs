using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalWaterMarkApp
{
    /**
        Класс водяного знака
        Содержит методы следующей и предыдущений
        трансформации преобразованием Арнольда
    */
    public class WaterMark : IEnumerable<WaterMarkItem>
    {
        private int size;
        private List<WaterMarkItem> items;

        public int Size {
            get => size;
            set => size = value;
        }

        public List<WaterMarkItem> Items {
            get => items;
            set => items = value;
        }

        public void Add(WaterMarkItem item) {
            this.items.Add(item);
        }

        public void NextIterateArnlodTransform() {
            foreach (WaterMarkItem item in this.Items)
            {
                var newColIdx = (2 * item.ColIdx + item.RowIdx) % size + 1;
                var newRowIdx = (item.ColIdx + item.RowIdx) % size + 1;

                item.RowIdx = newRowIdx;
                item.ColIdx = newColIdx;
            }

            this.Items.Sort();
        }

        public void PreviousIterateArnlodTransform() {
            foreach (WaterMarkItem item in this.Items)
            {
                item.ColIdx--;
                item.RowIdx--;

                var newRowIdx = (item.ColIdx - item.RowIdx) % size;
                var newColIdx = (-item.ColIdx + 2 * item.RowIdx) % size;

                if (newColIdx <= 0) newColIdx += size;
                if (newRowIdx <= 0) newRowIdx += size;

                item.RowIdx = newRowIdx;
                item.ColIdx = newColIdx;
            }

            this.Items.Sort();
        }

        public void IterateKTransforms(int k) {
            for (int i = 0; i < k; i++)
            {
                NextIterateArnlodTransform();
            }
        }

        public int this[int index]
        {
            get => this.Items[index].WMValue;
            set => this.Items[index].WMValue = value;
        }

        public IEnumerator<WaterMarkItem> GetEnumerator()
        {
            foreach (WaterMarkItem item in this.Items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public WaterMark(
            List<WaterMarkItem> items,
            int size
        ) {
            this.items = items;
            this.size = size;
        }

        public WaterMark(int size) {
            this.items = new List<WaterMarkItem>();
            this.size = size;
        }
    }
}