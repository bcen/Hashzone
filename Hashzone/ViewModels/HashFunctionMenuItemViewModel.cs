using System;
using System.Windows.Input;
using Hashzone.Infrastructure;

namespace Hashzone.ViewModels
{
    public class HashFunctionMenuItemViewModel : ViewModelBase
    {
        #region Property

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName == value)
                    return;

                _displayName = value;
                NotifyPropertyChanged("DisplayName");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;

                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        private ICommand _hashFuncChangeCommand;
        public ICommand HashFuncChangeCommand
        {
            get
            {
                if (_hashFuncChangeCommand == null)
                    _hashFuncChangeCommand = new RelayCommand(() =>
                    {
                        App.Notification.NotifyColleagues("HashFuncChangeExecute");
                        App.Notification.NotifyColleagues("HashFuncChanged", Name);
                        IsChecked = true;
                    });

                return _hashFuncChangeCommand;
            }
        }

        #endregion // Property

        #region Constructor

        public HashFunctionMenuItemViewModel()
            : this(String.Empty, String.Empty, false)
        {
        }

        public HashFunctionMenuItemViewModel(string displayName, string name, bool isChecked)
        {
            _displayName = displayName;
            _name = name;
            _isChecked = isChecked;
            if (_isChecked)
                App.Notification.NotifyColleagues("HashFuncChanged", Name);
        }

        #endregion // Constructor
    }
}
