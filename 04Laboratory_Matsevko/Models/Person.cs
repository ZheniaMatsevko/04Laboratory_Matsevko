using _04Laboratory_Matsevko.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _04Laboratory_Matsevko.Models
{
    class Person : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _dateOfBirth;

        private bool _isAdult;
        private bool _isBirthday;
        private WestZodiacSigns _sunSign;
        private ChineseZodiacSigns _chineseSign;

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public DateTime DateOfBirth { get => _dateOfBirth; 
            set { 
                
                _dateOfBirth = value;
            }
        }

        public ChineseZodiacSigns ChineseSign { get { return _chineseSign; } set { _chineseSign = value; } }
        public WestZodiacSigns SunSign { get { return _sunSign; } set { _sunSign = value; } }

        public bool IsAdult { get { return _isAdult; } set { _isAdult = value; } }

        public bool IsBirthday { get { return _isBirthday; } set { _isBirthday = value; } }

        public Person() { }
        public Person(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName)
        {
            _email = email;
        }
        public Person(string firstName, string lastName, DateTime dateOfBirth, string email = "") : this(firstName, lastName, email)
        {
            _dateOfBirth = dateOfBirth;
        }

        public void Proceed()
        {
            _isAdult = WorkWithDate.isAdult(_dateOfBirth);
            _isBirthday = WorkWithDate.isBirthday(_dateOfBirth);
            _chineseSign = WorkWithDate.calculateChineseZodiacSign(_dateOfBirth);
            _sunSign = WorkWithDate.calculateWestZodiacSign(_dateOfBirth);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
