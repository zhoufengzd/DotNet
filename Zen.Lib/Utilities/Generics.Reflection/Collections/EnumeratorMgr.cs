using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// 4:47 PM 2/20/2010. Finally, grouper / enumerator work fine after one week of exhausted time!
    /// 
    /// WARNING:  Enumerator is **NOT** Iterator. 
    /// IEnumerator is designed pooly. 
    ///    Enumerator starts in invalid state. 
    ///    MoveNext MUST be called first, then you get an valid object if it's not empty.
    ///    It's ok to only have single enumerator. But once you want handle multiple ones, what a pain!
    /// </summary>
    internal sealed class EnumeratorMgr : IEnumerator<IEnumerable<IEnumerator>>
    {
        public EnumeratorMgr()
        {
            _enumerators = new List<IEnumerator>();
        }
        public void Dispose() { }

        public void AddEnumerable(IEnumerable enumerable)
        {
            _enumerators.Add(enumerable.GetEnumerator());
        }
        public int Count
        {
            get { return _enumerators.Count; }
        }

        public IEnumerable<IEnumerator> Current
        {
            get { return _enumerators; }
        }
        object IEnumerator.Current
        {
            get { return _enumerators; }
        }

        public bool MoveNext()
        {
            if (_iterationCount++ == 0)
                 return DoMoveFirst();
            else
                return DoMoveNext();
        }

        public void Reset()
        {
            foreach (IEnumerator ie in _enumerators)
                ie.Reset();

            _iterationCount = 0;
            _subIndex = _enumerators.Count - 1;
            _mainIndex = _subIndex - 1;
        }

        #region private functions
        private bool DoMoveFirst()
        {
            _subIndex = _enumerators.Count - 1;
            _mainIndex = _subIndex - 1;

            foreach (IEnumerator ie in _enumerators)
            {
                if (!ie.MoveNext())
                    return false;
            }
            return true;
        }

        /// <summary> After that, only move a single one enumerator every time </summary>
        private bool DoMoveNext()
        {
            if (MoveSub())
                return true;

            if (MoveMain())
            {
                ResetSub(_mainIndex + 1);
                return MoveSub();
            }

            return false;
        }

        private bool MoveSub()
        {
            while (_subIndex > -1)
            {
                IEnumerator ie = _enumerators[_subIndex];
                if (ie.MoveNext())
                    return true;

                _subIndex--;
                if (_subIndex <= _mainIndex)
                    break;
            }

            return false;
        }

        private bool MoveMain()
        {
            for (; _mainIndex > -1; _mainIndex--)
            {
                IEnumerator ie = _enumerators[_mainIndex];
                if (ie.MoveNext())
                    return true;
            }

            return false;  
        }

        private void ResetSub(int startLoc)
        {
            Debug.Assert(startLoc < _enumerators.Count);

            for (int i = startLoc; i < _enumerators.Count; i++)
            {
                IEnumerator ie = _enumerators[i];
                ie.Reset();
            }

            _subIndex = _enumerators.Count - 1;
        }

        #endregion

        #region private data
        private List<IEnumerator> _enumerators;
        private int _mainIndex = -1; // always going upper level
        private int _subIndex = -1;  // reset if _mainIndex moved
        private int _iterationCount = 0;
        #endregion
    }
}
