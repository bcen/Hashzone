using System;
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
        private string _status = String.Empty;
        private string _droppedFilePath = String.Empty;

        public MainWindowViewModel()
        {
            // Block.Empty;
            _status = "Drag 'n' Drop file into the hash zone.";
        }

        public void HandleFileDrop(string[] paths)
        {
            Status = "Hashing file. . .";
            _droppedFilePath = paths[0];
            Thread t = new Thread(new ThreadStart(DoHashFile));
            t.Start();
        }

        private void DoHashFile()
        {
            try
            {
                Status = "SHA1: " + HashUtil.HashFile(_droppedFilePath);
            }
            catch (Exception ex)
            {
                Status = "Unable to hash the file.";
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
    }
}
