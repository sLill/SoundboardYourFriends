using System;
using System.Windows.Input;

namespace SoundboardYourFriends.Core
{
    public class DelegateCommand : ICommand
    {
        #region Member Variables..
        private CommandEventHandler _handler;
        #endregion Member Variables..

        #region Properties..
        #region IsEnabled
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }

            set
            {
                _isEnabled = value;
                OnCanExecuteChanged();
            }
        }
        #endregion IsEnabled
        #endregion Properties..

        #region Delegates/Events..
        public delegate void CommandEventHandler(object commandParameter);
        public event EventHandler CanExecuteChanged;
        #endregion Delegates/Events..

        #region Constructors..
        #region DelegateCommand
        public DelegateCommand(CommandEventHandler handler)
        {
            _handler = handler;
        } 
        #endregion DelegateCommand
        #endregion Constructors..

        #region Methods..
        #region Event Handlers..
        #region OnCanExecuteChanged
        private void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        #endregion OnCanExecuteChanged
        #endregion Event Handlers..				

        #region CanExecute
        bool ICommand.CanExecute(object arg)
        {
            return IsEnabled;
        }
        #endregion CanExecute

        #region Execute
        void ICommand.Execute(object arg)
        {
            _handler(arg);
        }
        #endregion Execute
        #endregion Methods..
    }
}
