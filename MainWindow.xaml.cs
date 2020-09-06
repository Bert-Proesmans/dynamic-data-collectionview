using DynamicData;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;

namespace DynamicDataCollectionView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompositeDisposable _reactiveCleanup;
        private ReadOnlyObservableCollection<Person> _flatList;
        private ReadOnlyObservableCollection<PersonAgeGroup> _groupList;

        public MainWindow()
        {
            InitializeComponent();
            _reactiveCleanup = null;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if(_reactiveCleanup != null)
            {
                return;
            }

            _reactiveCleanup = new CompositeDisposable();
            SetupReactive();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if(_reactiveCleanup == null)
            {
                return;
            }

            _reactiveCleanup.Dispose();
            _reactiveCleanup = null;
        }

        private void SetupReactive()
        {
            var dispatcher = Application.Current.Dispatcher;

            var localList = CreatePeopleObservable()
                        .ToObservableChangeSet()
                        .AsObservableList();

            var groupLoader = localList.Connect()
                .GroupOn(person => person.Age)
                .Transform(group => new PersonAgeGroup(group, dispatcher))
                .ObserveOn(dispatcher)
                .Bind(out _groupList)
                .Subscribe();


            var peopleLoader = localList.Connect()
                .ObserveOn(dispatcher)
                .Bind(out _flatList)
                .Subscribe();

            _reactiveCleanup.Add(peopleLoader);
            _reactiveCleanup.Add(groupLoader);

            var flatView = new ListCollectionView(_flatList);
            flatView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Person.Age)));

            FlatCollection.ItemsSource = flatView;
            DynamicData.ItemsSource = new DynamicDataView(_groupList);
        }

        private IObservable<Person> CreatePeopleObservable()
        {
            var people = new[]
            {
                new Person("JMN", 1),
                new Person("Andrea", 1),
                new Person("Ana", 1),
                new Person("Pepito", 2),
                new Person("Johnny", 4),
                new Person("Mary", 1),
                new Person("Rose", 4),
                new Person("Anthony", 3),
                new Person("David", 2),
                new Person("Joanna", 5),
                new Person("Oscar", 4),
                new Person("Tom", 5),
                new Person("Rachel", 3),
                new Person("Robert", 3),
                new Person("Diana", 2),
                new Person("Emily", 1),
                new Person("William", 3),
                new Person("Sarah", 6),
            };

            var intervalObs = Observable.Interval(TimeSpan.FromSeconds(2));
            var peopleObs = people.ToObservable();
            var intervalPeopleObs = intervalObs.Zip(peopleObs, (_, person) => person);
            return intervalPeopleObs;
        }
    }
}
