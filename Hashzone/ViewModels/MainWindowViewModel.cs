using System;
using System.Threading;
using System.Windows;
using Hashzone.Util;

namespace Hashzone.ViewModels
{
    /// <summary>
    /// Main window's view model
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        // Private Properties //////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        private string _hashName;
        private string _status;
        private bool _allowDrop = true;

        private string[] _droppedFilePaths;
        private string _hashMessage;

        // Private Properties //////////////////////////////////////////////////////////////////////



        // Constructors ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

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

        // Constructors ////////////////////////////////////////////////////////////////////////////



        // Public //////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        public void HandleFileDropEvent(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _droppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                DoHash(_droppedFilePaths, DataFormats.FileDrop);
            }
            else if (e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                DoHash(e.Data.GetData(DataFormats.UnicodeText), DataFormats.UnicodeText);
            }
            else
            {
                Status = "What did you drop in?";
            }
        }

        // Public //////////////////////////////////////////////////////////////////////////////////



        // Private /////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        private void SetupNotification()
        {
            App.Notification.Register("HashFuncChanged", (Action<string>)(name =>
            {
                HashName = name;
            }));

            App.Notification.Register("PasteExecuted", (Action<string>)(hashMsg =>
            {
                if (hashMsg.Trim().ToLower().Equals(_hashMessage))
                    Status = "Valid";
                else
                    Status = "Invalid";
            }));
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

        // Private /////////////////////////////////////////////////////////////////////////////////



        // Properties //////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

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

        // Properties //////////////////////////////////////////////////////////////////////////////
    }
}
