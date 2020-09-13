using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Data;
using System.Windows.Threading;

namespace DynamicDataCollectionView
{
    public class DynamicDataView : DispatcherObject, IEnumerable, INotifyCollectionChanged, ICollectionView, INotifyPropertyChanged, IItemProperties
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly ReadOnlyObservableCollection<PersonAgeGroup> _originalCollection;
        private readonly ObservableCollection<ItemPropertyInfo> _itemPropertiesEditable;
        private readonly ReadOnlyObservableCollection<ItemPropertyInfo> _itemProperties;
        private readonly ObservableCollection<GroupDescription> _groupDescriptionsEditable;
        private readonly ObservableCollection<object> _groupsEditable;
        private readonly ReadOnlyObservableCollection<object> _groups;
        private List<int> _indexers;

        public DynamicDataView(ReadOnlyObservableCollection<PersonAgeGroup> collection)
        {
            _originalCollection = collection;

            _itemPropertiesEditable = new ObservableCollection<ItemPropertyInfo>();
            _itemProperties = new ReadOnlyObservableCollection<ItemPropertyInfo>(_itemPropertiesEditable);
            _groupDescriptionsEditable = new ObservableCollection<GroupDescription>();
            _groupsEditable = new ObservableCollection<object>();
            _groups = new ReadOnlyObservableCollection<object>(_groupsEditable);
            _indexers = new List<int>();

            {
                var properties = typeof(Person).GetProperties();
                foreach (var property in properties)
                {
                    var name = property.Name;
                    var type = property.PropertyType;
                    var descriptor = property;
                    _itemPropertiesEditable.Add(new ItemPropertyInfo(name, type, descriptor));
                }
            }

            {
                _groupDescriptionsEditable.Add(new PropertyGroupDescription(nameof(Person.Age)));
            }

            {
                foreach (var group in _originalCollection)
                {
                    _groupsEditable.Add(group);
                }
                ((INotifyCollectionChanged)_originalCollection).CollectionChanged += DynamicDataView_CollectionChanged;
            }
        }

        private void DynamicDataView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var copy = _originalCollection.ToList();
            Dispatcher.Invoke(() =>
            {
                _groupsEditable.Clear();
                foreach (var group in copy)
                {
                    _groupsEditable.Add(new CollectionViewGroupProxy(group, Dispatcher));
                }
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
        }

        public bool CanFilter => false;

        public bool CanGroup { get; }

        public bool CanSort => false;

        public CultureInfo Culture { get; set; }

        public object CurrentItem => GetCurrentItem();

        public int CurrentPosition { get => throw new NotImplementedException(); }

        public Predicate<object> Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObservableCollection<GroupDescription> GroupDescriptions => _groupDescriptionsEditable;

        public ReadOnlyObservableCollection<object> Groups => _groups;

        public bool IsCurrentAfterLast { get => throw new NotImplementedException(); }

        public bool IsCurrentBeforeFirst { get => throw new NotImplementedException(); }

        public bool IsEmpty => _originalCollection.Count == 0;

        public SortDescriptionCollection SortDescriptions => SortDescriptionCollection.Empty;

        public IEnumerable SourceCollection { get => _originalCollection; }

        public ReadOnlyCollection<ItemPropertyInfo> ItemProperties => _itemProperties;

        private object GetCurrentItem() => null;

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
            foreach (var group in _originalCollection)
            {
                var copy = group.ToList();
                yield return copy.AsEnumerable();
            }
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
