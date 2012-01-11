using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Hashzone.Infrastructure;

namespace Hashzone.ViewModels
{
    public class ContextMenuViewModel : ViewModelBase
    {
        private bool _useMD5;
        private bool _useSHA1;
        private bool _canCopy;
        private string _hashMessage;
        private ICommand _md5Command;
        private ICommand _sha1Command;
        private ICommand _copyCommand;
        public ObservableCollection<HashFunction> HashFuncList { get; set; }

        public ContextMenuViewModel()
        {
            _useSHA1 = true;

            App.Notification.Register("CAN_COPY", (Action<bool>)(canCopy =>
            {
                _canCopy = canCopy;
            }));
            App.Notification.Register("HASH_MESSAGE", (Action<string>)(hashMessage =>
            {
                _hashMessage = hashMessage;
            }));

            HashFuncList = new ObservableCollection<HashFunction>();
            HashFuncList.Add(new HashFunction("MD5", "MD5"));
            HashFuncList.Add(new HashFunction("SHA-1", "SHA1"));
        }

        public ICommand MD5Command
        {
            get
            {
                if (_md5Command == null)
                    _md5Command = new RelayCommand(MD5Execute);
                return _md5Command;
            }
        }

        public ICommand SHA1Command
        {
            get
            {
                if (_sha1Command == null)
                    _sha1Command = new RelayCommand(SHA1Execute);
                return _sha1Command;
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand(CopyExecute, CopyCanExecute);
                return _copyCommand;
            }
        }

        private void MD5Execute()
        {
            UseMD5 = true;
            UseSHA1 = false;

            App.Notification.NotifyColleagues("USE_MD5");
        }

        private void SHA1Execute()
        {
            UseMD5 = false;
            UseSHA1 = true;

            App.Notification.NotifyColleagues("USE_SHA1");
        }

        private void CopyExecute()
        {
            Clipboard.SetText(_hashMessage);
        }

        private bool CopyCanExecute()
        {
            App.Notification.NotifyColleagues("COPY_CAN_EXE");
            return _canCopy;
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
