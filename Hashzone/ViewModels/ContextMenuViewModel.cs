using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hashzone.Infrastructure;

namespace Hashzone.ViewModels
{
    /// <summary>
    /// The view model for the context menu.
    /// </summary>
    public class ContextMenuViewModel : ViewModelBase
    {
        // Private Properties///////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        private string _messageToCopy;
        private string _messageToPaste;
        private ICommand _copyCommand;
        private ICommand _pasteCommand;
        private ObservableCollection<HashFunctionMenuItemViewModel> _hashFuncMenuItemList;

        private bool _canCopy;

        // Private Properties///////////////////////////////////////////////////////////////////////



        // Constructors/////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////    

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContextMenuViewModel()
            : this(new ObservableCollection<HashFunctionMenuItemViewModel>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the ContextMenuViewModel class with a list of 
        /// hash function menu items.
        /// </summary>
        /// <param name="hashFuncMenuItemList"></param>
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

        // Constructors/////////////////////////////////////////////////////////////////////////////



        // Private//////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

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

        // Private Methods//////////////////////////////////////////////////////////////////////////



        // Properties///////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets or Sets the message to be copied.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the message to be pasted.
        /// </summary>
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

        /// <summary>
        /// Returns a copy command.
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

        /// <summary>
        /// Returns a paste command.
        /// </summary>
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

        /// <summary>
        /// Returns a observable collection of hash function context menu item.
        /// </summary>
        public ObservableCollection<HashFunctionMenuItemViewModel> HashFuncMenuItemList
        {
            get { return _hashFuncMenuItemList; } 
        }

        // Properties///////////////////////////////////////////////////////////////////////////////
    }
}
