using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynamicDataCollectionView
{
    public class Person : INotifyPropertyChanged
    {
        private string _name;
        private int _age;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get => _name; set => SetAndRaise(ref _name, value); }
        public int Age { get => _age; set => SetAndRaise(ref _age, value); }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        private void SetAndRaise<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            where T : IEquatable<T>
        {
            if (field != null && field.Equals(value))
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Equals(Person other)
        {
            return string.Equals(Name, other.Name) && Age == other.Age;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Age;
            }
        }
    }
}
