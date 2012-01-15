using System;
using System.Windows.Input;
using Hashzone.Infrastructure;

namespace Hashzone.ViewModels
{
    public class HashFunctionMenuItemViewModel : ViewModelBase
    {
        // Private Properties///////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

        private string _displayName;
        private string _name;
        private bool _isChecked;
        private ICommand _hashFuncChangeCommand;

        // Private Properties///////////////////////////////////////////////////////////////////////



        // Constructors/////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////   

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

        // Constructors/////////////////////////////////////////////////////////////////////////////



        // Properties///////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////

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

        // Properties///////////////////////////////////////////////////////////////////////////////
    }
}
