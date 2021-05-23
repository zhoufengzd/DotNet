using System.Collections.Generic;
using System.Threading;

namespace Zen.Utilities.Threading
{
    /// <summary>
    /// Controller dash board. 
    ///   Expose IControllable interface
    ///   Provided default state control logic 
    /// Note: Derived class has the responsibility to set state properly after each action.
    /// </summary>
    public class ControllerBase : IControllable
    {
        public ControllerBase()
        {
        }

        public bool Start()
        {
            RequestAction(ActionRequest.Start);
            return true;
        }
        public bool Stop()
        {
            RequestAction(ActionRequest.Stop);
            return true;
        }
        public bool Pause()
        {
            RequestAction(ActionRequest.Pause);
            return true;
        }
        public State State
        {
            get { return _currentState; }
        }

        /// <summary> Return null if no action allowed. </summary>
        public ActionRequest[] GetActionsAllowed(State gs)
        {
            LoadStateMap();
            return _stateMap[gs];
        }

        #region protected functions
        protected bool IsActionAllowed(ActionRequest action)
        {
            ActionRequest[] allowedActions = GetActionsAllowed(_currentState);
            foreach (ActionRequest act in allowedActions)
            {
                if (act == action)
                    return true;
            }

            return false;
        }

        protected virtual State RequestAction(ActionRequest request)
        {
            if (!IsActionAllowed(request))
                return _currentState;

            _latestRequest = request;
            if (_latestRequest == ActionRequest.Start)
            {
                if (_currentState != State.Paused)
                {
                    if (_runThreadPool)
                        ThreadRunner.Run(DoRun);
                    else
                        DoRun();
                }
            }
            return _currentState;
        }

        protected virtual void DoRun()
        {
        }
        #endregion

        #region private functions
        private void LoadStateMap()
        {
            if (_stateMap != null)
                return;

            _stateMap = new Dictionary<State, ActionRequest[]>();
            _stateMap.Add(State.Paused, new ActionRequest[] { ActionRequest.Start, ActionRequest.Stop });
            _stateMap.Add(State.Pending, null);
            _stateMap.Add(State.Running, new ActionRequest[] { ActionRequest.Pause, ActionRequest.Stop });
            _stateMap.Add(State.Stopped, new ActionRequest[] { ActionRequest.Start });
        }
        #endregion                                                                                                   

        #region protected data
        protected Dictionary<State, ActionRequest[]> _stateMap;
        protected bool _runThreadPool = true;
        protected State _currentState = State.Stopped;
        protected ActionRequest _latestRequest = ActionRequest.Unspecified;
        #endregion
    }

}
