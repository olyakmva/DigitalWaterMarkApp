using SupportLib;

namespace AlgorithmsLibrary
{
    public class PointPercentCriterion : ICriterion
    {
        private int _neededPointNumber;
        private int _currentPointNumber;
        private int _initPointNumber;
        private int _errorValue;
        private int _prevPointNum;
        private double _prevTolerance;

        private int _n ;

        public void GetParamByCriterion(SimplificationAlgmParameters options)
        {
            if (_n == 1 )
            {
                _prevTolerance = options.Tolerance;
                _prevPointNum = _currentPointNumber;
                options.Tolerance =
                    Math.Round(options.Tolerance * ((double) _currentPointNumber / (double) _neededPointNumber));
            }
            else
            {
                var p2 = options.Tolerance;
                if (_currentPointNumber == _prevPointNum)
                {
                    options.Tolerance =
                        Math.Round(p2 * ((double) _currentPointNumber / (double) _neededPointNumber));
                }
                else
                {
                    options.Tolerance = Math.Round(
                        (p2 * (_prevPointNum - _neededPointNumber) -
                         _prevTolerance * (_currentPointNumber - _neededPointNumber)) /
                        (_prevPointNum - _currentPointNumber));
                    if (options.Tolerance < 0)
                    {
                        options.Tolerance =
                            Math.Round(p2 *
                                       ((double) _currentPointNumber / (double) _neededPointNumber));
                    }
                }
                _prevTolerance = p2;
                _prevPointNum = _currentPointNumber;
            }
        }

        public void Init(MapData initMap, SimplificationAlgmParameters options)
        {
            _initPointNumber = initMap.Count;
            _neededPointNumber = Convert.ToInt32(Math.Round( _initPointNumber * options.RemainingPercent/100));
            _errorValue = Convert.ToInt32(Math.Round(_initPointNumber * options.PointNumberGap / 100));
            if(options.Tolerance <1)
                options.Tolerance=100;
            _prevPointNum = 0;
            _prevTolerance = 0;
            _n = 0;
            _currentPointNumber = 0;
        }

        public bool IsSatisfy(MapData map)
        {
            _n++;
            _currentPointNumber = map.Count;
            if (Math.Abs(_currentPointNumber - _neededPointNumber) <= _errorValue)
            {
                return true;
            }
            return false;
        }
    }


}
