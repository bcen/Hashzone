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
        }

        public void HandleFileDropEvent(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _droppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                Thread t = new Thread(new ThreadStart(DoHashFile));
                t.Start();
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
                Status = (UseMD5 ? "MD5: " : "SHA1: ") + HashUtil.HashFile(_droppedFilePaths[0],
                                                                           UseMD5 ? "MD5" : "SHA1");
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

        public ICommand MD5Command
        {
            get { return new RelayCommand(MD5Execute); }
        }

        public ICommand SHA1Command
        {
            get { return new RelayCommand(SHA1Execute); }
        }

        void MD5Execute()
        {
            UseMD5 = true;
            UseSHA1 = false;
            Status = "MD5 selected.";
        }

        void SHA1Execute()
        {
            UseMD5 = false;
            UseSHA1 = true;
            Status = "SHA1 selected.";
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

        public bool UseMD5
        {
            get { return _useMD5; }
            set
            {
                _useMD5 = value;
                NotifyPropertyChanged("UseMD5");
            }
        }

        public bool UseSHA1
        {
            get { return _useSHA1; }
            set
            {
                _useSHA1 = value;
                NotifyPropertyChanged("UseSHA1");
            }
        }
    }
}
