using _04Laboratory_Matsevko.Models;
using _04Laboratory_Matsevko.Services;
using _04Laboratory_Matsevko.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace _04Laboratory_Matsevko.ViewModels
{
    class PageViewModel : INotifyPropertyChanged
    {
        private Person _user = new Person();
        private RelayCommand<object> _proceedCommand;
        private DateTime? _chosenDate;
        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _user.FirstName; }
            set { _user.FirstName = value; }
        }

        public string LastName
        {
            get { return _user.LastName; }
            set { _user.LastName = value; }
        }

        public string Email
        {
            get { return _user.Email; }
            set { _user.Email = value; }
        }

        private void validateEmail(string email)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!Regex.IsMatch(email, regex, RegexOptions.IgnoreCase))
                throw new InvalidEmailException(email);
        }

        public string DateOfBirth
        {
            get { return (_user.SunSign == WestZodiacSigns.None) ? "" : _user.DateOfBirth.ToShortDateString(); }
            set { OnPropertyChanged(); }
        }

        public string WestZodiacSign
        {
            get { return (_user.SunSign == WestZodiacSigns.None) ? "" : _user.SunSign.ToString(); }
            set { OnPropertyChanged(); }
        }

        public string IsAdult
        {
            get { return (_user.ChineseSign == ChineseZodiacSigns.None) ? "" : _user.IsAdult.ToString(); }
            set { OnPropertyChanged(); }
        }
        public string IsBirthday
        {
            get { return (_user.ChineseSign == ChineseZodiacSigns.None) ? "" : _user.IsBirthday.ToString(); }
            set { OnPropertyChanged(); }
        }

        public string ChineseZodiacSign
        {
            get { return (_user.ChineseSign == ChineseZodiacSigns.None) ? "" : _user.ChineseSign.ToString(); }
            set { OnPropertyChanged(); }
        }

        public RelayCommand<object> ProceedCommand
        {
            get
            {
                return _proceedCommand ??= new RelayCommand<object>(_ => Proceed(), CanExecute);
            }
        }

        private bool CanExecute(object obj)
        {
            return _chosenDate != DateTime.Now && !String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName) && !String.IsNullOrWhiteSpace(Email);
        }

        private async void Proceed()
        {

            try
            {
                validateEmail(Email);
                WorkWithDate.checkDate(_chosenDate.Value);
            }
            catch (InvalidEmailException ex)
            {
                _user.Email = "";
                OnPropertyChanged("Email");
                MessageBox.Show(ex.Message);
                return;
            }
            catch (PersonIsTooOldException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (DateInFutureException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _user.DateOfBirth = _chosenDate.Value;
            IsEnabled = false;
            await Task.Run(() => doAsyncOperation());
            if (_chosenDate.Value.Day == DateTime.Today.Day && _chosenDate.Value.Month == DateTime.Today.Month)
                MessageBox.Show("Happy birthday!!!");
        }

        private void doAsyncOperation()
        {
            //Thread.Sleep(3000);
            _user.Proceed();
            showInfo();
            WestZodiacSign = _user.SunSign.ToString();
            ChineseZodiacSign = _user.ChineseSign.ToString();
            IsAdult = _user.IsAdult.ToString();
            IsBirthday = _user.IsBirthday.ToString();
            IsEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime? ChosenDate
        {
            get
            {
                return _chosenDate ??= DateTime.Today;
            }
            set
            {
                if (_chosenDate.Value.CompareTo(value) != 0)
                    _chosenDate = value.Value;
            }
        }

        private void showInfo()
        {
            OnPropertyChanged("FirstName");
            OnPropertyChanged("LastName");
            OnPropertyChanged("DateOfBirth");
            OnPropertyChanged("Email");
        }
    }
}
