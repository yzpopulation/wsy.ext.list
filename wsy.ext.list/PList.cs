#region using
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace wsy.ext.list
{
    public class PList<T> : List<T>
    {
        private readonly int _stopcapacity;
        
        public PList(int capacity) : base(capacity)
        {
            _stopcapacity = capacity;
        }

        public PList()
        {
            _stopcapacity = -1;
        }

        /// <summary>
        ///     删除事件
        /// </summary>
        public event EventHandler<NListEventArgs<T>> OnDeleteBefore;
        public event EventHandler<NListEventArgs<T>> OnDeleteAfter;
        /// <summary>
        ///     添加事件
        /// </summary>
        public event EventHandler<NListEventArgs<T>> OnInsertBefore;
        public event EventHandler<NListEventArgs<T>> OnInsertAfter;
        /// <summary>
        ///     清空事件
        /// </summary>
        public event EventHandler<NListEventArgs<IEnumerable<T>>> OnClearBefore;
        public event EventHandler<NListEventArgs<IEnumerable<T>>> OnClearAfter;
        /// <summary>
        ///     Range事件
        /// </summary>
        public event EventHandler<NListEventArgs<IEnumerable<T>>> OnRangeBefore;
        public event EventHandler<NListEventArgs<IEnumerable<T>>> OnRangeAfter;
        public new void Add(T item)
        {
            OnInsertBefore?.Invoke(this, new NListEventArgs<T>(item, Count));
            if (_stopcapacity > 0 && Count >= _stopcapacity) Clear();
            base.Add(item);
            OnInsertAfter?.Invoke(this, new NListEventArgs<T>(item, Count));
        }
        public new void Insert(int index, T item)
        {
            OnInsertBefore?.Invoke(this, new NListEventArgs<T>(item, -1));
            if (_stopcapacity > 0 && Count >= _stopcapacity) Clear();
            base.Insert(index, item);
            OnInsertAfter?.Invoke(this, new NListEventArgs<T>(item, index));
        }
        public new void Remove(T item)
        {
            OnDeleteBefore?.Invoke(this, new NListEventArgs<T>(item, IndexOf(item)));
            base.Remove(item);
            OnDeleteAfter?.Invoke(this, new NListEventArgs<T>(default(T), Count));
        }
        public new void RemoveAt(int index)
        {
            var item = base[index];
            OnDeleteBefore?.Invoke(this, new NListEventArgs<T>(item, index));
            base.RemoveAt(index);
            OnDeleteAfter?.Invoke(this, new NListEventArgs<T>(default(T), Count));
        }
        public new void RemoveRange(int index, int count)
        {
            OnRangeBefore?.Invoke(this, new NListEventArgs<IEnumerable<T>>(GetRange(index, count), count));
            base.RemoveRange(index, count);
            OnRangeAfter?.Invoke(this, new NListEventArgs<IEnumerable<T>>(default(IEnumerable<T>), Count));
        }
        public new void AddRange(IEnumerable<T> collection)
        {
            OnRangeBefore?.Invoke(this, new NListEventArgs<IEnumerable<T>>(collection, collection.Count()));
            base.AddRange(collection);
            OnRangeAfter?.Invoke(this, new NListEventArgs<IEnumerable<T>>(this, Count));
        }
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            OnRangeBefore?.Invoke(this, new NListEventArgs<IEnumerable<T>>(collection, collection.Count()));
            if (_stopcapacity > 0 && Count >= _stopcapacity) Clear();
            base.InsertRange(index, collection);
            OnRangeAfter?.Invoke(this, new NListEventArgs<IEnumerable<T>>(this, Count));
        }
        public new void Clear()
        {
            OnClearBefore?.Invoke(this, new NListEventArgs<IEnumerable<T>>(this, Count));
            base.Clear();
            OnClearAfter?.Invoke(this, new NListEventArgs<IEnumerable<T>>(this, Count));
        }
    }

    public class NListEventArgs<T> : EventArgs
    {
        public NListEventArgs(T item, int index)
        {
            Item  = item;
            Index = index;
        }
        public T   Item  { get; set; }
        public int Index { get; set; }
    }
}