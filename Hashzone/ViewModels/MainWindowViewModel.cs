﻿using System;
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
        }

        public void HandleFileDropEvent(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                HandleFileDrop((string[])e.Data.GetData(DataFormats.FileDrop));
            }
            else if (e.Data.GetDataPresent(DataFormats.UnicodeText)
                        || e.Data.GetDataPresent(DataFormats.Text))
            {
                string hexString = (string)e.Data.GetData(DataFormats.UnicodeText);
                if (hexString.Length != 40)
                {
                    Status = "It doesn't seem like a hex string.";
                }
            }
            else
            {
                Status = "The drop does not associate with any file type.";
            }
        }

        public void HandleFileDrop(string[] paths)
        {
            _droppedFilePaths = paths;
            Thread t = new Thread(new ThreadStart(DoHashFile));
            t.Start();
        }

        private void DoHashFile()
        {
            try
            {
                AllowDrop = false;
                Status = "Hashing file. . .";
                Status = "SHA1: " + HashUtil.HashFile(_droppedFilePaths[0]);
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
