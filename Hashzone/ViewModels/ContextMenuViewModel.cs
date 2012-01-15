using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
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

        #endregion // Declaration


        #region Property

        public string MessageToCopy
        {
            get { return _messageToCopy; }
            set
            {
                if (_messageToCopy == value)
                    return;

                _messageToCopy = value;
                NotifyPropertyChanged("MessageToCopy");
            }
        }
        private string _messageToCopy;

        public string MessageToPaste
        {
            get { return _messageToPaste; }
            set
            {
                if (_messageToPaste == value)
                    return;

                _messageToPaste = value;
                NotifyPropertyChanged("MessageToPaste");
            }
        }
        private string _messageToPaste;

        /// <summary>
        /// Copy command for "Copying" the status text.
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand(() =>
                    {
                        Clipboard.SetText(MessageToCopy);
                    }, () => _canCopy);

                return _copyCommand;
            }
        }
        private ICommand _copyCommand;

        public ICommand PasteCommand
        {
            get
            {
                string hashMsg = String.Empty;
                if (_pasteCommand == null)
                {
                    _pasteCommand = new RelayCommand(() =>
                    {
                        App.Notification.NotifyColleagues("PasteExecuted", hashMsg);
                    }, () =>
                    {
                        hashMsg = Clipboard.GetText(TextDataFormat.UnicodeText);
                        MessageToPaste = hashMsg;
                        return !String.IsNullOrEmpty(hashMsg);
                    });
                }

                return _pasteCommand;
            }
        }
        private ICommand _pasteCommand;

        public ObservableCollection<HashFunctionMenuItemViewModel> HashFuncMenuItemList
        {
            get { return _hashFuncMenuItemList; } 
        }
        private ObservableCollection<HashFunctionMenuItemViewModel> _hashFuncMenuItemList;

        #endregion // Property


        #region Constructor

        public ContextMenuViewModel()
            : this(new ObservableCollection<HashFunctionMenuItemViewModel>())
        {
        }

        public ContextMenuViewModel(
            ObservableCollection<HashFunctionMenuItemViewModel> hashFuncMenuItemList)
        {
            _messageToCopy = String.Empty;
            _messageToPaste = String.Empty;

            _hashFuncMenuItemList = hashFuncMenuItemList;
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("MD5", "MD5", true));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-1", "SHA1", false));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-256 Managed", "SHA256", 
                                                                        false));
            _hashFuncMenuItemList.Add(new HashFunctionMenuItemViewModel("SHA-384 Managed", "SHA384",
                                                                        false));

            SetupNotification();
        }

        #endregion // Constructor


        #region Private Method

        private void SetupNotification()
        {
            App.Notification.Register("HashFuncChangeExecute", () =>
            {
                foreach (HashFunctionMenuItemViewModel h in _hashFuncMenuItemList)
                    h.IsChecked = false;
            });

            App.Notification.Register("CanCopyMessage", (Action<string>)(msg =>
            {
                MessageToCopy = msg;
                _canCopy = true;
            }));
        }

        #endregion // Private Method
    }
}
