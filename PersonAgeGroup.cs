using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Threading;

namespace DynamicDataCollectionView
{
    public class PersonAgeGroup : IDisposable, IGrouping<int, Person>
    {
        private readonly CompositeDisposable _cleanup;
        private readonly ReadOnlyObservableCollection<Person> _group;

        public PersonAgeGroup(IGroup<Person, int> group, Dispatcher dispatcher)
        {
            _cleanup = new CompositeDisposable();

            var peopleLoader = group.List.Connect()
                .ObserveOn(dispatcher)
                .Bind(out _group)
                .Subscribe();

            _cleanup.Add(peopleLoader);

            Key = group.GroupKey;
        }

        public ReadOnlyObservableCollection<Person> Group => _group;

        public int Key { get; }

        public void Dispose()
        {
            _cleanup.Dispose();
        }

        public IEnumerator<Person> GetEnumerator() => _group.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _group.GetEnumerator();
    }
}
