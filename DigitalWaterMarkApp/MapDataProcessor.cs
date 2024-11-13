using System.Numerics;
using SupportLib;

namespace DigitalWaterMarkApp {

    /// <summary>
    /// Класс обработчик карты
    /// </summary>
    public class MapDataProcessor {
        private WaterMark waterMark;

        # region Константы функции универсального хэширования

        private static BigInteger A = 35;
        private static BigInteger B = 84;

        private static BigInteger P = 95;

        #endregion

        public WaterMark WaterMark {
            get => waterMark;
            set => waterMark = value;
        }

        public MapDataProcessor(WaterMark waterMark) {
            this.waterMark = waterMark;
        }

        /// <summary>
        /// Метод внедрения водяного знака путем изменения порядка хранения картографических объектов
        /// Вычисление позиции внедряемого бита ЦВЗ основано на вычислении фнукции универсального хэширования с заранее заданными константами
        /// Необходимость смены направления хранения определяется путем сравнения вычесленной позиции с текущим направлением хранения
        /// Текущее направление хранения двух рядом лежащих объектов вычсиляется как результат сравнения числа точек в каждом из объектов
        /// Прямое направление хранения объектов будем определять как 1, если число точек текущего объекта больше количества точек в следующем
        /// Обратное направление хранения двух объектов определяется как 0 в противоположном случае
        /// После смены направления хранения текущего объекта итерируем объекты карты дальше
        /// </summary>
        /// <param name="mapData">Объект карты, прочитанный из shp файла</param>
        /// <returns>Объект карты с внедренным ЦВЗ</returns>
        public MapData WaterMarkEmbedding(MapData mapData) {

            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkEmbeddingItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count, this.WaterMark.Length);
                var waterMarkEmbeddingItem = this.waterMark[waterMarkEmbeddingItemIdx];
                var storageDirection = GetMapObjectsStorageDirtection(mapData.MapObjDictionary[i].Value, mapData.MapObjDictionary[i + 1].Value);

                if (waterMarkEmbeddingItem != storageDirection) {
                    mapData.SwapMapObjects(i, i + 1);
                }
            }

            return mapData;
        }

        /// <summary>
        /// Метод извлечения ЦВЗ с применением функции хэширования используемой при внедрении
        /// Входными параметрами являются карта с внедренным ЦВЗ и длина извлекаемого водяного знака
        /// Метод извлечения водяного знака для каждой пары объектов определяет следующие значения:
        ///     - индекс бита извлекаемого ЦВЗ как результат вычисления хэширующей функции
        ///     - значение бита извлекаемого ЦВЗ как направление хранения текущих двух объектов
        /// Поскольку объектов карты может быть сильно больше чем длина ЦВЗ, на каждый индекс извлекаемого ЦВЗ
        /// может быть вычислено множество значений 0 или 1. Поэтому итоговое значение на каждом индекс водяного знака
        /// определяется методов "голосования большинства", т.е. результирует значение, преобладающее большее число раз
        /// </summary>
        /// <param name="mapData">Объект карты с внедренным ЦВЗ</param>
        /// <param name="waterMarkLength">Длина извлекаемого ЦВЗ</param>
        /// <returns>Извлеченный водяной знак</returns>
        public static int[] WaterMarkExtracting(MapData mapData, int waterMarkLength) {

            Dictionary<int, List<int>> waterMarkValues = new();
            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkExtractionItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count, waterMarkLength);
                var waterMarkExtractionItem = GetMapObjectsStorageDirtection(mapData.MapObjDictionary[i].Value, mapData.MapObjDictionary[i + 1].Value);

                if (waterMarkValues.ContainsKey(waterMarkExtractionItemIdx)) {
                    waterMarkValues[waterMarkExtractionItemIdx].Add(waterMarkExtractionItem);
                } else {
                    waterMarkValues.Add(waterMarkExtractionItemIdx, new List<int>(waterMarkExtractionItem));
                }
            }

            var maxIndex = waterMarkValues.Keys.Max();
            var waterMarkBestVariation = Enumerable.Repeat(-1, maxIndex + 1).ToArray();
            foreach (var wmKey in waterMarkValues.Keys)
            {
                var countOf_1 = waterMarkValues[wmKey].Count(item => item == 1);
                var countOf_0 = waterMarkValues[wmKey].Count(item => item == 0);

                if (countOf_1 > countOf_0) waterMarkBestVariation[wmKey] = 1;
                else if (countOf_0 > countOf_1) waterMarkBestVariation[wmKey] = 0;
            }

            return waterMarkBestVariation;
        }

        /// <summary>
        /// Метод вычисляющий значение функции универсального хэширования
        /// На вход передается целое число и ограничивающий модуль, т.е. длина ЦВЗ
        /// </summary>
        /// <param name="value">Входное значение</param>
        /// <param name="waterMarkLength">Длина ЦВЗ как ограничивающий модуль</param>
        /// <returns>Вычисленный хэш</returns>
        private static int GetHash(int value, int waterMarkLength) => (int) (((A * value + B) % P) % waterMarkLength);

        /// <summary>
        /// Метод, определяющий направление хранения двух объектов
        /// Направление хранения двух объектов вычсиляется как результат сравнения числа точек в каждом из объектов
        /// Прямое направление хранения объектов будем определять как 1, если число точек текущего объекта больше количества точек в следующем
        /// Обратное направление хранения двух объектов определяется как 0 в противоположном случае
        /// </summary>
        /// <param name="firstMapObject">Первый объект карты</param>
        /// <param name="secondMapObject">Второй объект карты</param>
        /// <returns>Вычисленное направление хранения</returns>
        private static int GetMapObjectsStorageDirtection(
            List<MapPoint> firstMapObject,
            List<MapPoint> secondMapObject
        ) {
            return firstMapObject.Count <= secondMapObject.Count ? 0 : 1;
        }

        /*
        * Ниже методы, описывающие альтернативный способ внедрения/извлечения ЦВЗ, осноанный на
        * дублировании точек внутри картографических объектов.
        * Этот метод не содержит вероятностных функций и является более устойчивым поскольку превосходит предыдущий метод
        * при атаках путем делеции/инсерции объектов карты
        */

        /// <summary>
        /// Метод дублирования точек внутри картографического объекта
        /// На вход передается позиция, которая также служит периодом дублирования, т.е.
        /// все точки начиная с переданной позиции с периодом в значение позиции однократно дублируются
        /// </summary>
        /// <param name="position">Позиция дублирования точки</param>
        /// <param name="objectId">Идентификатор картографического объекта внутри карты</param>
        /// <param name="mapData">Объект карты</param>
        private static void LoopDuplicatingPointsAtIndex(int position, int objectId, MapData mapData) {

            if (position == 0) {
                return;
            }

            int[] positionsForDuplicating = new int[mapData[objectId].Count / position];
            int currentIndex = 0;
            for (int i = position; i < mapData[objectId].Count + currentIndex && currentIndex < positionsForDuplicating.Length; i += position) {
                positionsForDuplicating[currentIndex] = i - 1 + currentIndex++;
            }

            for (int i = 0; i < positionsForDuplicating.Length; i++) {
                mapData[objectId].Insert(positionsForDuplicating[i], mapData[objectId][positionsForDuplicating[i]]);
            }
        }

        /// <summary>
        /// Метод вычисляющий максимальный размер объекта среди всех объектов карты
        /// Определяется как максимально возможное значение числа точек среди всех объектов карты
        /// </summary>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Максимальный размер объекта</returns>
        private static int MaxObjectSizeInMapData(MapData mapData) {
            var maximumPossibleWMvalue = -1;

            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                int countPointsInObject = mapObject.Value.Count;

                if (countPointsInObject > maximumPossibleWMvalue) {
                    maximumPossibleWMvalue = countPointsInObject;
                }
            }

            maximumPossibleWMvalue -= 2;

            Console.WriteLine(string.Format("Max possible watermark value: {0}", maximumPossibleWMvalue));
            return maximumPossibleWMvalue;
        }

        /// <summary>
        /// Метод поиска позиций, значения которых продублированы в картографическом объекте
        /// Перебирает все точки объекта и запоминает позицию на которых обнаружена первая из дублированных точек
        /// </summary>
        /// <param name="objectId">Идентификатор картографического объекта</param>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Список индексов продублированных точек в объекте карты</returns>
        private static List<int> FindLoopingPositionsInMapDataObject(int objectId, MapData mapData) {

            List<int> duplicatingIndexes = new();

            for (int i = 1; i < mapData[objectId].Count; i++) {
                var previousItem = mapData[objectId][i - 1];
                var currentItem = mapData[objectId][i];

                if (previousItem.CompareTo(currentItem) == 0) {
                    duplicatingIndexes.Add(i - 1);
                }
            }

            return duplicatingIndexes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mapData"></param>
        public void WaterMarkEmbeddingViaLoopingDuplicateOfPoints(MapData mapData) {
            var maximumPossibleWMvalue = MaxObjectSizeInMapData(mapData);
            long wmDecimal = this.waterMark.ConvertToDecimal();

            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                int countPointsInObject = mapObject.Value.Count;

                long periodAsPosition;
                if (wmDecimal > maximumPossibleWMvalue) {
                    periodAsPosition = wmDecimal % countPointsInObject;
                } else {
                    periodAsPosition = countPointsInObject % wmDecimal;
                }

                LoopDuplicatingPointsAtIndex((int) periodAsPosition, objectId, mapData);
            }
        }

        private static (int loopingPosition, int countPointsBeforeLooping) FindEquationPropsForLayer(int objectId, MapData mapData) {
            var loopingIndexes = FindLoopingPositionsInMapDataObject(objectId, mapData);
            var countPointsBeforeLooping = mapData[objectId].Count - loopingIndexes.Count;

            if (loopingIndexes.Count == 0) {
                return (-1, -1);
            }

            var loopingPosition = loopingIndexes[0] + 1;

            if (loopingPosition == 1) {
                return (-1, -1);
            }

            return (loopingPosition, countPointsBeforeLooping);
        }

        private static int FindDifferenceForLooping(int objectId, MapData mapData) {

            var props = FindEquationPropsForLayer(objectId, mapData);

            if (Math.Min(props.loopingPosition, props.countPointsBeforeLooping) == -1) {
                return -1;
            }

            return props.countPointsBeforeLooping - props.loopingPosition;
        }

        private static List<(int loopingPosition, int countPointsBeforeLooping)> FindAllEquationPropsInLayers(MapData mapData) {
            List<(int loopingPosition, int countPointsBeforeLooping)> mapLayerLoopingEquationProps = new();
            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                var currentProp = FindEquationPropsForLayer(objectId, mapData);
                if (Math.Min(currentProp.loopingPosition, currentProp.countPointsBeforeLooping) != -1) {
                    mapLayerLoopingEquationProps.Add(currentProp);
                }
            }
            return mapLayerLoopingEquationProps;
        }

        private static List<int> FindAllDifferencesInLayers(MapData mapData) {
            List<int> mapLayerLoopingDifferences = new();
            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                var currentDefference = FindDifferenceForLooping(objectId, mapData);
                if (currentDefference != -1) {
                    mapLayerLoopingDifferences.Add(currentDefference);
                }
            }
            return mapLayerLoopingDifferences;
        }

        static BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y) {
            if (b == 0) {
                x = 1;
                y = 0;
                return a;
            }

            BigInteger gcd = ExtendedGCD(b, a % b, out x, out y);
            BigInteger temp = x;
            x = y;
            y = temp - (a / b) * y;

            return gcd;
        }

        private static long GCDOfList(List<int> numbers) {
            if (numbers == null || numbers.Count == 0)
                throw new ArgumentException("List cannot be null or empty.");

            BigInteger gcdResult = numbers[0];

            for (int i = 1; i < numbers.Count; i++)
            {
                gcdResult = ExtendedGCD(gcdResult, numbers[i], out BigInteger _, out BigInteger _);
                if (gcdResult == 1)
                    return 1;
            }

            return (long) gcdResult;
        }

        private static BigInteger Product(int[] A) {
            BigInteger product = 1;
            foreach (int a in A) {
                product *= a;
            }
            return product;
        }

        private static long ChineseRemainderTheorem(int[] A, int[] B) {
            if (A.Length != B.Length)
                throw new ArgumentException("The lengths of A and B must be the same.");

            BigInteger M = Product(A);
            BigInteger x = 0;

            for (int i = 0; i < A.Length; i++) {
                long ai = A[i];
                long bi = B[i];
                BigInteger Mi = M / ai;
                long yi = ModularInverse(Mi, ai);

                x += bi * Mi * yi;
            }

            return (long) (x % M);
        }

        private static long ModularInverse(BigInteger a, BigInteger m) {
            BigInteger gcd = ExtendedGCD(a, m, out BigInteger x, out BigInteger y);

            if (gcd != 1) {
                throw new ArgumentException("Ошибка поиска обратного элемента.");
            }

            var inverseItem = (x % m + m) % m;
            return (long) inverseItem;
        }

        private static List<(int loopingPosition, int countPointsBeforeLooping)>
            FindPairwiseMutuallyPrimeNumbers(List<(int loopingPosition, int countPointsBeforeLooping)> equationProps) {
            var resultEquationProps = new List<(int loopingPosition, int countPointsBeforeLooping)>();
            for (int i = 0; i < equationProps.Count - 1; i++) {
                int currentNumber = equationProps[i].countPointsBeforeLooping;
                for (int j = i + 1; j < equationProps.Count; j++) {
                    int nextNumber = equationProps[j].countPointsBeforeLooping;
                    if (ExtendedGCD(currentNumber, nextNumber, out BigInteger _, out BigInteger _) != 1) {
                        equationProps.RemoveAt(j--);
                    }
                }
            }
            return equationProps;
        }

        public static WaterMark FindWMDecimalFromLoopingsInMapData(MapData mapData) {
            // В случае MaxObjectSizeInMapData > WM_10
            // Решение системы вида:
            // A_{0} mod x ≡ B_{0}
            // A_{1} mod x ≡ B_{1}
            // ...
            // A_{n} mod x ≡ B_{n}
            long WMViaDifferences = GCDOfList(FindAllDifferencesInLayers(mapData));

            // В случае MaxObjectSizeInMapData < WM_10
            // x mod A_{0} ≡ B_{0}
            // x mod A_{1} ≡ B_{1}
            // ...
            // x mod A_{n} ≡ B_{n}
            if (WMViaDifferences == 1) {
                var equationProps = FindPairwiseMutuallyPrimeNumbers(FindAllEquationPropsInLayers(mapData));

                var countPointsBeforeLoopingArray = equationProps.Select(propItem => propItem.countPointsBeforeLooping).ToArray();
                var loopingPositionArray = equationProps.Select(propItem => propItem.loopingPosition).ToArray();

                long WMViaCRT = ChineseRemainderTheorem(countPointsBeforeLoopingArray, loopingPositionArray);
                return WaterMark.ConvertToWaterMark(WMViaCRT);
            }

            return WaterMark.ConvertToWaterMark(WMViaDifferences);
        }
    }
}