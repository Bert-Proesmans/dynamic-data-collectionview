using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

namespace DynamicDataCollectionView
{
    public class CollectionViewGroupProxy : CollectionViewGroup
    {
        private readonly PersonAgeGroup _wrapped;
        private readonly Dispatcher _dispatcher;
        public CollectionViewGroupProxy(PersonAgeGroup group, Dispatcher dispatcher): base(group.Key)
        {
            _wrapped = group;
            _dispatcher = dispatcher;

            ((INotifyCollectionChanged)_wrapped.Group).CollectionChanged += CollectionViewGroupProxy_CollectionChanged;
            {
                foreach(var item in _wrapped.Group)
                {
                    ProtectedItems.Add(item);
                }
                ProtectedItemCount = group.Group.Count;
            }
        }

        private void CollectionViewGroupProxy_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var copy = _wrapped.Group.ToList();
            _dispatcher.Invoke(() =>
            {
                ProtectedItems.Clear();
                foreach(var item in copy)
                {
                    ProtectedItems.Add(item);
                }
                ProtectedItemCount = copy.Count;
            });
        }

        public override bool IsBottomLevel => true;
    }
}
