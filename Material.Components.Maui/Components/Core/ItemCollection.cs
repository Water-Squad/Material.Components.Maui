﻿using System.Collections.ObjectModel;

namespace Material.Components.Maui.Core;

public class ItemsChangedEventArgs<T>
{
    public string EventType { get; set; }

    public int Index { get; set; }

    public T Item { get; set; }
}

public class ItemCollection<T> : ObservableCollection<T>
{
    public event EventHandler<ItemsChangedEventArgs<T>> OnAdded;
    public event EventHandler<ItemsChangedEventArgs<T>> OnRemoved;
    public event EventHandler OnCleared;

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);

        OnAdded?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Insert",
            Index = index,
            Item = item
        });
    }

    protected override void RemoveItem(int index)
    {
        T item = this.Items[index];

        base.RemoveItem(index);

        OnRemoved?.Invoke(this, new ItemsChangedEventArgs<T>
        {
            EventType = "Remove", 
            Index = index,
            Item = item
        });
    }

    protected override void ClearItems()
    {
        base.ClearItems();
        OnCleared?.Invoke(this, default);
    }
}
