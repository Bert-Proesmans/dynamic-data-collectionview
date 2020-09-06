using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Threading;

namespace DynamicDataCollectionView
{
    public class DynamicDataView : DispatcherObject, IEnumerable, INotifyCollectionChanged, ICollectionView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly ReadOnlyObservableCollection<PersonAgeGroup> _originalCollection;
        private List<int> _indexers;

        public DynamicDataView(ReadOnlyObservableCollection<PersonAgeGroup> collection)
        {
            _originalCollection = collection;
            _indexers = new List<int>();
        }

        public bool CanFilter => false;

        public bool CanGroup { get; }

        public bool CanSort => false;

        public CultureInfo Culture { get; set; }

        public object CurrentItem => GetCurrentItem();

        public int CurrentPosition { get => throw new NotImplementedException(); }

        public Predicate<object> Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObservableCollection<GroupDescription> GroupDescriptions => new ObservableCollection<GroupDescription>();

        public ReadOnlyObservableCollection<object> Groups { get => throw new NotImplementedException(); }

        public bool IsCurrentAfterLast { get => throw new NotImplementedException(); }

        public bool IsCurrentBeforeFirst { get => throw new NotImplementedException(); }

        public bool IsEmpty { get => throw new NotImplementedException(); }

        public SortDescriptionCollection SortDescriptions => SortDescriptionCollection.Empty;

        public IEnumerable SourceCollection { get => _originalCollection; }

        private object GetCurrentItem()
        {
            if(_indexers.Count == 0)
            {
                // MoveCurrentToFirst(); // Circular dependency?
                _indexers.AddRange(new[] { 0, 0 });
            }


            if(_originalCollection.Count > 0)
            {
                var first = _originalCollection[_indexers[0]];
                if(first.Group.Count > 0)
                {
                    var second = first.Group[_indexers[1]];
                    return second;
                }
            }

            return null;
        }

        public bool Contains(object item)
        {
            throw new NotImplementedException();
        }

        public IDisposable DeferRefresh()
        {
            // TODO
            return Disposable.Create(() => { });
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentTo(object item)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToFirst()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToLast()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToNext()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPosition(int position)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPrevious()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
