using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hashzone.Infrastructure;

namespace Hashzone.ViewModels
{
    /// <summary>
    /// View model for the context menu on main window.
    /// </summary>
    public class ContextMenuViewModel : ViewModelBase
    {
        #region Declaration

        private bool _canCopy;
        private string _messageToCopy;

        #endregion // Declaration

        #region Property

        private ICommand _copyCommand;
        /// <summary>
        /// Copy command for "Copying" the status text.
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand(CopyExecute, CopyCanExecute);
                return _copyCommand;
            }
        }

        private ObservableCollection<HashFunctionMenuItemViewModel> _hashFuncMenuItemList;
        public ObservableCollection<HashFunctionMenuItemViewModel> HashFuncMenuItemList
        { get { return _hashFuncMenuItemList; } }

        #endregion // Property

        #region Constructor

        public ContextMenuViewModel()
            : this(new ObservableCollection<HashFunctionMenuItemViewModel>())
        {
        }

        public ContextMenuViewModel(
            ObservableCollection<HashFunctionMenuItemViewModel> hashFuncMenuItemList)
        {
            _hashFuncMenuItemList = hashFuncMenuItemList;
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("MD5", "MD5", false));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-1", "SHA1", true));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-256 Managed", "SHA256", 
                                                                        false));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-384 Managed", "SHA384",
                                                                        false));

            SetupNotification();
        }

        #endregion

        #region Private Method

        private void SetupNotification()
        {
            App.Notification.Register("HashFuncChangeExecute", (Action<string>)(name =>
            {
                foreach (HashFunctionMenuItemViewModel h in _hashFuncMenuItemList)
                    h.IsChecked = false;
            }));
        }

        private void CopyExecute()
        {

        }

        private bool CopyCanExecute()
        {
            return _canCopy;
        }

        #endregion
    }
}
