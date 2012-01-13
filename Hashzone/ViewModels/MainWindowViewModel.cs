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

        #endregion // Declaration

        #region Property

        private string _hashName;
        public string HashName
        {
            get { return _hashName; }
            set
            {
                if (_hashName == value)
                    return;

                _hashName = value;
                NotifyPropertyChanged("HashName");
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                    return;

                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private bool _allowDrop = true;
        public bool AllowDrop
        {
            get { return _allowDrop; }
            set
            {
                if (_allowDrop == value)
                    return;

                _allowDrop = value;
                NotifyPropertyChanged("AllowDrop");
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
                //Thread t = new Thread(new ThreadStart(DoHashFile));
                //t.Start();
                DoHash(_droppedFilePaths, DataFormats.FileDrop);
            }
            else if (e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                //AllowDrop = false;
                //string message = HashUtil.HashString((string)e.Data.GetData(
                //                                     DataFormats.UnicodeText), _hashName);
                //_hashMessage = message;
                //Status = _hashName + ": " + message;
                //AllowDrop = true;
                //App.Notification.NotifyColleagues("CanCopyMessage", _hashMessage);
                DoHash(e.Data.GetData(DataFormats.UnicodeText), DataFormats.UnicodeText);
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
                HashName = name;
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

        private void DoHash(object data, string format)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    AllowDrop = false;
                    Status = "Hashing. . .";
                    string message = String.Empty;
                    if (format.Equals(DataFormats.FileDrop))
                    {
                        message = HashUtil.HashFile(((string[])data)[0], _hashName);
                    }
                    else if (format.Equals(DataFormats.UnicodeText))
                    {
                        message = HashUtil.HashString((string)data, _hashName);
                    }
                    _hashMessage = message;
                    Status = _hashName + ": " + message;
                    App.Notification.NotifyColleagues("CanCopyMessage", _hashMessage);
                }
                catch (Exception e)
                {
                    Status = "Unable to hash the file.";
                }
                finally
                {
                    AllowDrop = true;
                }
            }));
            t.Start();
        }

        #endregion // Private Method
    }
}
