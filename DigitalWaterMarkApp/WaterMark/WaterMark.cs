using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWaterMarkApp {
    /**
        Класс водяного знака
        Содержит методы следующей и предыдущений
        трансформации преобразованием Арнольда
    */
    public class WaterMark : IEnumerable<WaterMarkItem> {
        private int size;
        private List<WaterMarkItem> items;

        private static readonly int bitsPerSymbol = 8;

        public int Size {
            get => size;
            set => size = value;
        }

        public int Length {
            get => size * size;
        }

        public List<WaterMarkItem> Items {
            get => items;
            set => items = value;
        }

        public void Add(WaterMarkItem item) {
            this.items.Add(item);
        }

        public void NextIterateArnlodTransform() {
            foreach (WaterMarkItem item in this.Items) {
                var newColIdx = (2 * item.ColIdx + item.RowIdx) % size + 1;
                var newRowIdx = (item.ColIdx + item.RowIdx) % size + 1;

                item.RowIdx = newRowIdx;
                item.ColIdx = newColIdx;
            }

            this.Items.Sort();
        }

        public void PreviousIterateArnlodTransform() {
            foreach (WaterMarkItem item in this.Items) {
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
            for (int i = 0; i < k; i++) {
                NextIterateArnlodTransform();
            }
        }

        private static int[] GetBitArrayFromBigInteger(int length, string number)
        {
            var source = number.PadLeft(length, '0');
            return source.Select(c => c - '0').ToArray();
        }

        public static String ConvertToBase(long value, int toBase) => Convert.ToString(value, toBase);

        public BigInteger ConvertToDecimal() {
            BitArray bits = new(this.Items.Select(x => x.WMValue > 0).ToArray());

            byte[] bytes = new byte[(bits.Length + (bitsPerSymbol - 1)) / bitsPerSymbol];
            bits.CopyTo(bytes, 0);
            
            return new(bytes);
        }

        public static WaterMark ConvertToWaterMark(BigInteger secretCodeBigInteger) {

            if (secretCodeBigInteger == 0)
                throw new ArgumentException("Значение ЦВЗ не может быть равным 0");

            string bigIntegerOfBits = "";
            while (secretCodeBigInteger > 0) {
                bigIntegerOfBits = (secretCodeBigInteger % 2) + bigIntegerOfBits;
                secretCodeBigInteger /= 2;
            }
            var size = GetSizeToNearestSquare(bigIntegerOfBits.Length);
            int[] binary = GetBitArrayFromBigInteger(size * size, bigIntegerOfBits).Reverse().ToArray();
            return ConvertToWaterMark(binary, size);
        }

        private static int GetSizeToNearestSquare(int currentLength) {
            int size = 0;
            for (int i = 1; i < 100; i++) {
                if (currentLength <= i * i) {
                    size = i;
                    break;
                }
            }
            return size;
        }

        public static WaterMark ConvertToWaterMark(string secretCode) {

            byte[] secretCodeInBytes = Encoding.GetEncoding(1251).GetBytes(secretCode);
            string[] byteStrings = secretCodeInBytes.Select(_byte => Convert.ToString(_byte, 2).PadLeft(bitsPerSymbol, '0')).ToArray();
            string stringOfBits = string.Join("", byteStrings);
            int bigIntegerOfBitsLength = stringOfBits.Length;
            var size = GetSizeToNearestSquare(bigIntegerOfBitsLength);
            int[] binary = GetBitArrayFromBigInteger(size * size, stringOfBits);
            return ConvertToWaterMark(binary, size);
        }

        private static WaterMark ConvertToWaterMark(int[] binary, int size) {

            List<WaterMarkItem> items = new();

            int currentItem = 0;
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    items.Add(new WaterMarkItem(binary[currentItem], i, j));
                    currentItem++;
                }
            }

            return new WaterMark(items, size);
        }

        public string ToSecretCode() {
            int[] binary = this.Items.Select(x => x.WMValue).TakeLast(FindNearestMultipleOfBitsPerSymbolLessThan(this.Items.Count)).ToArray();
            string asciiString = "";

            int counter = 0;
            byte[] bytes = new byte[(binary.Length + (bitsPerSymbol - 1)) / bitsPerSymbol];
            for (int i = 0; i < binary.Length; i += bitsPerSymbol) {
                int byteValue = 0;
                for (int j = 0; j < bitsPerSymbol; j++) 
                    byteValue = (byteValue << 1) | binary[i + j]; // Сдвигаем влево и добавляем текущий бит
                bytes[counter++] = (byte) byteValue;
            };

            return Encoding.GetEncoding(1251).GetString(bytes);
        }

        private int FindNearestMultipleOfBitsPerSymbolLessThan(int number) {
            if (number <= 0)
                return 0;

            return number - (number % bitsPerSymbol);
        }

        public int this[int index] {
            get => this.Items[index].WMValue;
            set => this.Items[index].WMValue = value;
        }

        public IEnumerator<WaterMarkItem> GetEnumerator() {
            foreach (WaterMarkItem item in this.Items) {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
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