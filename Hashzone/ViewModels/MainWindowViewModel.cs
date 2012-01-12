using System;
using System.Threading;
using System.Windows;
using Hashzone.Util;

namespace Hashzone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Declaration

        private string[] _droppedFilePaths;
        private string _hashMessage;
        private string _hashName;

        #endregion // Declaration

        #region Property

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value && !_status.Equals(value))
                {
                    _status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private bool _allowDrop = true;
        public bool AllowDrop
        {
            get { return _allowDrop; }
            set
            {
                if (_allowDrop != value && !_allowDrop.Equals(value))
                {
                    _allowDrop = value;
                    NotifyPropertyChanged("AllowDrop");
                }
            }
        }

        #endregion // Property

        #region Constructor

        public MainWindowViewModel()
            : this("Drag 'n' Drop file into the hash zone.", true) 
        { 
        }

        public MainWindowViewModel(string status, bool allowDrop)
            : this(status, allowDrop, null)
        { 
        }

        public MainWindowViewModel(string status, bool allowDrop, string[] filePaths)
        {
            _status = status;
            _allowDrop = allowDrop;
            _droppedFilePaths = filePaths;

            SetupNotification();
        }

        #endregion // Constructor

        #region Public Method

        public void HandleFileDropEvent(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _droppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                Thread t = new Thread(new ThreadStart(DoHashFile));
                t.Start();
            }
            else if (e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                AllowDrop = false;
                string message = HashUtil.HashString((string)e.Data.GetData(
                                                     DataFormats.UnicodeText), _hashName);
                _hashMessage = message;
                Status = _hashName + ": " + message;
                AllowDrop = true;
                App.Notification.NotifyColleagues("CanCopyMessage", _hashMessage);
            }
            else
            {
                Status = "What did you drop in?";
            }
        }

        #endregion // Public Method

        #region Private Method

        private void SetupNotification()
        {
            App.Notification.Register("HashFuncChanged", (Action<string>)(name =>
            {
                _hashName = name;
            }));

            App.Notification.Register("PasteExecuted", (Action<string>)(hashMsg =>
            {
                if (hashMsg.Equals(_hashMessage))
                    Status = "Valid";
                else
                    Status = "Invalid";
            }));
        }

        private void DoHashFile()
        {
            try
            {
                AllowDrop = false;
                Status = "Hashing file. . .";
                string message = HashUtil.HashFile(_droppedFilePaths[0], _hashName);
                _hashMessage = message;
                Status = _hashName + ": " + message;
                App.Notification.NotifyColleagues("CanCopyMessage", _hashMessage);
            }
            catch (Exception ex)
            {
                Status = "Unable to hash the file.";
            }
            finally
            {
                AllowDrop = true;
            }
        }

        #endregion // Private Method
    }
}
