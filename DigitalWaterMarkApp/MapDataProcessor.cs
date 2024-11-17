using System.Numerics;
using SupportLib;

namespace DigitalWaterMarkApp {

    /// <summary>
    /// Класс обработчик карты
    /// </summary>
    public class MapDataProcessor {
        private WaterMark waterMark;

        # region Константы функции универсального хэширования

        private static readonly BigInteger A = 35, B = 84, P = 95;

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
            var minimumPossibleWMvalue = int.MaxValue;

            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                int countPointsInObject = mapObject.Value.Count;

                if (countPointsInObject > maximumPossibleWMvalue) {
                    maximumPossibleWMvalue = countPointsInObject;
                }

                if (countPointsInObject < minimumPossibleWMvalue) {
                    minimumPossibleWMvalue = countPointsInObject;
                }
            }

            // Вычитание необходимо для того, чтобы при выборе водяного знака со значением равным значению максимума или
            // меньшим чем значение максимума в 1 не происходило ошибок при решении систем
            maximumPossibleWMvalue -= 2;

            Console.WriteLine(string.Format("Max possible watermark value: {0}, min: {1}", maximumPossibleWMvalue, minimumPossibleWMvalue));
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
        /// Метод внедрения ЦВЗ в объект карты путем периодического дублирования точек в каждом объекте
        /// Первая позиция и последующий период (равный значению позиции) вычсиляется следующими способами:
        ///     - если десятичное представление ЦВЗ превышает максимально возможный размер объекта среди всех,
        ///       то позиция это остаток от деления десятичного значения ЦВЗ на размер соответсвующего объекта
        ///     - в противном случае берется остаток от деления размера соответсвующего объекта на десятичное значение ЦВЗ
        /// </summary>
        /// <param name="mapData">Объект карты</param>
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

        /// <summary>
        /// Извлечение ЦВЗ из объекта карты осуществляется следующими вариантами:
        /// Сперва рассматривается случай когда десятичное значение ЦВЗ перед внедрением было меньше (или равным) чем максимально возможный размер объекта среди всех
        /// В этом случае поиск значения ЦВЗ сводится к задаче решения системы вида A_{i} mod x ≡ B_{i} i=1..N, где N - число объектов карты,
        /// A_{i} - период дублирования точек, B_{i} - исходный размер объекта
        /// Необходимо найти общий модуль, который является значением ЦВЗ в десятичном представлении
        /// Решение такой системы ищется как НОД множества из объектов B_{i}-A_{i}
        ///
        /// Поскольку может быть случай когда задача не имеет решений то есть НОД = 1, то
        ///
        /// Из этого следует, что исходное значение ЦВЗ могло превышать максимально возможный размер объекта среди всех
        /// В этом случае остаток искался от деления значения ЦВЗ на размер каждого объекта
        /// То есть задача сводится к решению системы вида x mod B_{i} ≡ A_{i} i=1..N, где N - число объектов карты,
        /// A_{i} - период дублирования точек, B_{i} - исходный размер объекта
        /// Для решения системы такого вида сперва находим такие B_{i}, что НОД(B_{i},B_{j}) для любых i=1..N,j=1..N,i!=j = 1,
        /// то есть ищем множество в котором любые два элемента взаимно простые
        /// Затем для найденных пар A_{i}*, B_{i}*, i=1..M, M<=N применяем китайскую теорему об остатках
        /// Ее результат будет являться искомым значением ЦВЗ
        /// </summary>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Искомый ЦВЗ</returns>
        public static WaterMark FindWMDecimalFromLoopingsInMapData(MapData mapData) {
            // В случае MaxObjectSizeInMapData > WM_10
            // Решение системы вида:
            // A_{0} mod x ≡ B_{0}
            // A_{1} mod x ≡ B_{1}
            // ...
            // A_{n} mod x ≡ B_{n}
            long WMViaDifferences = GCDOfList(FindAllDifferencesInLayers(mapData));

            // В случае MaxObjectSizeInMapData < WM_10
            // x mod B_{0} ≡ A_{0}
            // x mod B_{1} ≡ A_{1}
            // ...
            // x mod B_{n} ≡ A_{n}
            if (WMViaDifferences == 1) {
                var equationProps = FindPairwiseMutuallyPrimeNumbers(FindAllEquationPropsInLayers(mapData));

                var countPointsBeforeLoopingArray = equationProps.Select(propItem => propItem.countPointsBeforeLooping).ToArray();
                var loopingPositionArray = equationProps.Select(propItem => propItem.loopingPosition).ToArray();

                long WMViaCRT = ChineseRemainderTheorem(countPointsBeforeLoopingArray, loopingPositionArray);
                return WaterMark.ConvertToWaterMark(WMViaCRT);
            }

            return WaterMark.ConvertToWaterMark(WMViaDifferences);
        }

        /// <summary>
        /// Метод возвращающий для объекта карты набор пар (A, B), где A - период дублирования точек в объекте, B - размер объекта перед внедрением ЦВЗ
        /// Сперва находятся все позции точки на которых были продублированы. Запоминается первая позиция, которая также соответсвует периоду дублирования
        /// Затем из числа точек объекта карты с дубликатами вычитается число точек, которые подверглись дублированию
        /// Найденная разность является числом точек в этом объекте до внедрения ЦВЗ
        /// </summary>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Пара (A, B), где A - период дублирования точек в объекте, B - размер объекта перед внедрением ЦВЗ</returns>
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

        /// <summary>
        /// Метод вовзращающий разность B-A для пары (A, B), где A - период дублирования точек в объекте ЦВЗ, B - размер объекта перед внедрением
        /// Является вспомогательным методом для решения системы вида A_{i} mod x ≡ B_{i} i=1..N, где N - число объектов карты
        /// </summary>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Искомая разность</returns>
        private static int FindDifferenceForLooping(int objectId, MapData mapData) {

            var props = FindEquationPropsForLayer(objectId, mapData);

            if (Math.Min(props.loopingPosition, props.countPointsBeforeLooping) == -1) {
                return -1;
            }

            return props.countPointsBeforeLooping - props.loopingPosition;
        }

        /// <summary>
        /// Метод возращающий для объекта карты список пар (A_{i}, B_{i}) i=1..N, где N - число объектов карты,
        /// A_{i} - период дублирования точек, B_{i} - размер объекта перед внедрением ЦВЗ в объекте
        /// </summary>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Список пар (A, B), где A - период дублирования точек в объекте, B - размер объекта перед внедрением ЦВЗ</returns>
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

        /// <summary>
        /// Возвращает для списка пар (A_{i}, B_{i}), i=1..N, где N - число объектов карты,
        /// A_{i} - период дублирования точек в объекте ЦВЗ, B_{i} - размер объекта перед внедрением
        /// список пар (A_{k}*, B_{k}*), где для НОД(B_{i'}*, B_{j'}*)=1, i'=1..N,j'=1..N,i'!=j',k=M,M<=N
        /// </summary>
        /// <param name="equationProps"></param>
        /// <returns>Спсиок пар</returns>
        private static List<(int loopingPosition, int countPointsBeforeLooping)>
            FindPairwiseMutuallyPrimeNumbers(List<(int loopingPosition, int countPointsBeforeLooping)> equationProps) {
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

        /// <summary>
        /// Возвращает список разностей B-A для списка пар (A, B), где A - период дублирования точек в объекте, B - размер объекта перед внедрением ЦВЗ
        /// Является вспомогательным методом для решения системы вида A_{i} mod x ≡ B_{i} i=1..N, где N - число объектов карты
        /// </summary>
        /// <param name="mapData">Объект карты</param>
        /// <returns>Список разностей B-A пар (A, B)</returns>
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

        /// <summary>
        /// Расширенный алгоритм Евклида
        /// На вход передаются числа a, b для поиска НОД
        /// И неприсвоенные значения x, y, которые будут являться коэффициентами Безу
        /// Поиск коээфициентов Безу необходим для нахождения обратного модуля в теореме КТО
        /// в случае решения системы вида x mod B_{i} ≡ A_{i} i=1..N, где N - число объектов карты,
        /// A_{i} - период дублирования точек, B_{i} - исходный размер объекта
        /// </summary>
        /// <returns>НОД(a, b)</returns>
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

        /// <summary>
        /// Метод упрощающий поиск НОД для списка чисел
        /// Поочередно для каждого числа считает НОД предыдущего вычисленного значения НОД и нового числа
        /// </summary>
        /// <param name="numbers">Список чисел для поиска НОД</param>
        /// <returns>НОД</returns>
        /// <exception cref="ArgumentException"></exception>
        private static long GCDOfList(List<int> numbers) {
            if (numbers == null || numbers.Count == 0)
                return 1;

            BigInteger gcdResult = numbers[0];

            for (int i = 1; i < numbers.Count; i++) {
                gcdResult = ExtendedGCD(gcdResult, numbers[i], out BigInteger _, out BigInteger _);
                if (gcdResult == 1)
                    return 1;
            }

            return (long) gcdResult;
        }

        /// <summary>
        /// Метод реализующий алгоритм китайской теоремы об остатках
        /// </summary>
        private static long ChineseRemainderTheorem(int[] A, int[] B) {
            if (A.Length != B.Length)
                throw new ArgumentException("The lengths of A and B must be the same.");

            // Произведение
            BigInteger M = A.Select(a => (BigInteger) a).Aggregate(BigInteger.Multiply);
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

        /// <summary>
        /// Метод реализующий алгоритм поиска обратного модульного элемента
        /// </summary>
        private static long ModularInverse(BigInteger a, BigInteger m) {
            BigInteger gcd = ExtendedGCD(a, m, out BigInteger x, out BigInteger y);

            if (gcd != 1) {
                throw new ArgumentException("Ошибка поиска обратного элемента.");
            }

            var inverseItem = (x % m + m) % m;
            return (long) inverseItem;
        }
    }
}