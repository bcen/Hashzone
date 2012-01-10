using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using Hashzone.Infrastructure;
using Hashzone.Util;

namespace Hashzone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _status;
        private bool _allowDrop = true;
        private string[] _droppedFilePaths;
        private bool _useMD5;
        private bool _useSHA1;
        private string _hashMessage;

        public MainWindowViewModel()
                : this(String.Empty, true) 
        { }

        public MainWindowViewModel(string status, bool allowDrop)
                : this(status, allowDrop, null)
        { }

        public MainWindowViewModel(string status, bool allowDrop, string[] filePaths)
        {
            _status = status;
            _allowDrop = allowDrop;
            _droppedFilePaths = filePaths;
            _useMD5 = false;
            _useSHA1 = true;

            App.Notification.Register("USE_MD5", UseMD5Callback);
            App.Notification.Register("USE_SHA1", UseSHA1Callback);
        }

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
                string hashName = _useMD5 ? "MD5" : "SHA1";
                string message = HashUtil.HashString((string)e.Data.GetData(
                                                     DataFormats.UnicodeText), hashName);
                _hashMessage = message;
                Status = hashName + ": " + message;
                AllowDrop = true;
            }
            else
            {
                Status = "What did you drop in?";
            }
        }

        private void DoHashFile()
        {
            try
            {
                AllowDrop = false;
                Status = "Hashing file. . .";
                string hashName = _useMD5 ? "MD5" : "SHA1";
                string message = HashUtil.HashFile(_droppedFilePaths[0], hashName);
                _hashMessage = message;
                Status = hashName + ": " + message;
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

        private void UseMD5Callback()
        {
            Console.WriteLine("UseMD5Callback");
            _useMD5 = true;
            _useSHA1 = false;
        }

        private void UseSHA1Callback()
        {
            Console.WriteLine("UseSHA1Callback");
            _useMD5 = false;
            _useSHA1 = true;
        }

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
    }
}
