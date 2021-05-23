
namespace Zen.Utilities.Algorithm
{
    /// <summary> comparison type </summary>
    public enum CompareType
    {
        NONE = 0,	// undefined
        GT = 1,		// greater then
        GT_EQL,		// greater then or equal
        EQL,	    // equal
        LT_EQL,		// less then or equal
        LT,			// less
    };

    public class FuzzyComparator
    {
        public FuzzyComparator(double dDelta)
        {
            _dDeltaPrcnt = -1.0;
            _dDelta = dDelta;
        }
        public FuzzyComparator(int percent, int percentageBase)
        {
            _dDeltaPrcnt = (percent / percentageBase);
            _dDelta = -1.0;
        }

        int compare(double left, double right)
        {
            double dDelta = (_dDelta > 0) ? _dDelta : _dDeltaPrcnt * right;
            if (left < (right - dDelta))
                return -1;
            else if (left > (right + dDelta))
                return 1;
            else
                return 0;
        }

        #region private data
        private double _dDeltaPrcnt;
        private double _dDelta;
        #endregion
    }

    public class Range
    {
        public Range()
        {
        }
        public Range(double dLowValue, double dHighValue)
        {
            _dLowValue = dLowValue;
            _dHighValue = dHighValue;

            System.Diagnostics.Debug.Assert((dLowValue <= dHighValue), "Invalid Range!");
        }
        public double Low
        {
            get { return _dLowValue; }
            set { _dLowValue = value; }
        }
        public double High
        {
            get { return _dHighValue; }
            set { _dHighValue = value; }
        }

        #region private data
        private double _dLowValue = 0.0;
        private double _dHighValue = 0.0;
        #endregion
    }

}
