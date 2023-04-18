using _04Laboratory_Matsevko.Models;
using _04Laboratory_Matsevko.Services;
using _04Laboratory_Matsevko.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace _04Laboratory_Matsevko.ViewModels
{
    class MainViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Person> _users;
        private ObservableCollection<Person> _filteredUsers;
        private ObservableCollection<Person> _originalUsers;
        private Person _selectedUser;
        private String _prevEditedEmail = "";
        private Person _prevEditedUser = null;
        private bool _is_checked = true;
        public String SortField { get; set; }
        public String FilterField { get; set; }
        public String FilterValue { get; set; }

        public bool Is_Checked { 
            get => _is_checked;
            set { _is_checked = value; OnPropertyChanged(); } }
     
        public Person SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                
                if (_prevEditedEmail.Length!=0  && value!=_selectedUser)
                {
                    if(checkData(_selectedUser, _prevEditedUser))
                    {
                        Is_Checked = true;

                        new UserService().Edit(_selectedUser, _prevEditedEmail);
                        _prevEditedEmail = "";
                        _prevEditedUser = null;
                        _selectedUser = value;
                    }   
                }
                else
                    _selectedUser = value;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Person> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        private void  tester()
        {
            List<Person> temp = new List<Person>();
            //Person person = new Person("Sasha", "Bessarab", new DateTime(2002, 12, 12), "sasha@gmail.com");
           // Person person = new Person("Sofiia", "yasnitska", new DateTime(1999, 4, 18), "sonik@gmail.com");
            Person person = new Person("Lesia", "Svyntuh", new DateTime(1972, 3, 2), "svyntuh@gmail.com");
            person.Proceed();
            // Person person1 = new Person("Ihor", "Kravets", new DateTime(2003, 11, 10), "ihor@gmail.com");
            //Person person1 = new Person("Andryi", "Khortyk", new DateTime(1978, 11, 10), "andr@gmail.com");
            Person person1 = new Person("Dmytro", "Popov", new DateTime(1988, 1, 1), "popov@gmail.com");
            person1.Proceed();
            temp.Add(person);
            temp.Add(person1);
            new UserService().Serialize(temp);
        }

        public MainViewModel()
        {

            //tester();
            _users = new ObservableCollection<Person>(new UserService().GetAllUsers());
            _originalUsers = _users;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Person _user = new Person();
        private RelayCommand<object> _insertCommand;
        private RelayCommand<object> _deleteCommand;
        private RelayCommand<object> _editCommand;
        private RelayCommand<object> _sortCommand;
        private RelayCommand<object> _filterCommand;
        private DateTime? _chosenDate;

        public string FirstName
        {
            get { return _user.FirstName; }
            set { _user.FirstName = value;OnPropertyChanged(); }
        }

        public string LastName
        {
            get { return _user.LastName; }
            set { _user.LastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _user.Email; }
            set { _user.Email = value;
                OnPropertyChanged();
            }
        }

        private void validateEmail(string email)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!Regex.IsMatch(email, regex, RegexOptions.IgnoreCase))
                throw new InvalidEmailException(email);
        }

        public string DateOfBirth
        {
            get { 
                return _user.DateOfBirth.ToShortDateString(); 
            }
            set { OnPropertyChanged(); }
        }



        public RelayCommand<object> InsertCommand
        {
            get
            {
                return _insertCommand ??= new RelayCommand<object>(_ => Proceed(), CanExecute);
            }
        }

        private bool CanExecute(object obj)
        {
            return _chosenDate != DateTime.Now && !String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName) && !String.IsNullOrWhiteSpace(Email);
        }

        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand<object>(_ => Delete(), CanExecuteDelete);
            }
        }

        private void Delete()
        {
            new UserService().RemoveOne(SelectedUser);
            _users.Remove(SelectedUser);
     
        }

        private bool CanExecuteDelete(object obj)
        {
            return SelectedUser!=null;
        }

        public RelayCommand<object> EditCommand
        {
            get
            {
                return _editCommand ??= new RelayCommand<object>(_ => Edit(), CanExecuteEdit);
            }
        }

        private void Edit()
        {
            Is_Checked = false;

            _prevEditedEmail = _selectedUser.Email;
            _prevEditedUser = new Person(_selectedUser.FirstName, _selectedUser.LastName, _selectedUser.DateOfBirth, _selectedUser.Email);
            _prevEditedUser.Proceed();
        }

        private bool CanExecuteEdit(object obj)
        {
            return SelectedUser != null;
        }

        public RelayCommand<object> SortCommand
        {
            get
            {
                return _sortCommand ??= new RelayCommand<object>(_ => Sort(), CanExecuteSort);
            }
        }

        private void Sort()
        {
            Users = new ObservableCollection<Person>(_users.OrderBy(o => o.GetType().GetProperty(SortField).GetValue(o)));
        }

        private bool CanExecuteSort(object obj)
        {
            if (SortField == null || SortField == "")
                return false;
            var props = typeof(Person).GetProperties();
            foreach(var p in props)
            {
                if (p.Name.ToLower().Equals(SortField.ToLower())) 
                {        
                    SortField = p.Name;
                    return true;
                }
                    
            }
            return false;
        }
        public RelayCommand<object> FilterCommand
        {
            get
            {
                return _filterCommand ??= new RelayCommand<object>(_ => Filter(), CanExecuteFilter);
            }
        }

        private void Filter()
        {
           
            _filteredUsers = new ObservableCollection<Person>(_users.Where(o => o.GetType().GetProperty(FilterField).GetValue(o).Equals(FilterValue))); 

            Users = _filteredUsers;
           
        }

        private bool CanExecuteFilter(object obj)
        {
            if (FilterField == null || FilterValue==null)
                return false;
            if(FilterValue=="" && FilterField == "")
            {
                Users = _originalUsers;
                return false;
            }
            var props = typeof(Person).GetProperties();
            foreach (var p in props)
            {
                if (p.Name.ToLower().Equals(FilterField.ToLower()))
                {
                    FilterField = p.Name;
                    return true;
                }

            }
            return false;
        }

        private bool checkData(Person person, Person prev)
        {
            try
            {
                validateEmail(person.Email);
                WorkWithDate.checkDate(person.DateOfBirth);
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message);
                person.Email = prev.Email;
                return false;
            }
            catch (PersonIsTooOldException ex)
            {
                MessageBox.Show(ex.Message);
                person.DateOfBirth = prev.DateOfBirth;
                return false;
            }
            catch (DateInFutureException ex)
            {
                MessageBox.Show(ex.Message);
                person.DateOfBirth = prev.DateOfBirth;
                return false;
            }
            person.Proceed();
            return true;
        }

        private void Proceed()
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
            _user.Proceed();
            _users.Add(_user);
            new UserService().SerializeOne(_user);
            _user = new Person();
            _chosenDate = null;
            clearInfo();

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

        private void clearInfo()
        {
            OnPropertyChanged("FirstName");
            OnPropertyChanged("LastName");
            OnPropertyChanged("ChosenDate");
            OnPropertyChanged("Email");
        }
    }
}
