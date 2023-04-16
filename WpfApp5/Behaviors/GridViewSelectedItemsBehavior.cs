using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp5.ViewModels;

namespace WpfApp5.Behaviors
{
    public class GridViewSelectedItemsBehavior : Behavior<DataGrid>
    {
        #region SelectedItems Property

        public List<MyItem> SelectedItems
        {
            get { return (List<MyItem>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(List<MyItem>), typeof(GridViewSelectedItemsBehavior), new UIPropertyMetadata(null, OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!ignoreOnChanged)
            {
                var grid = (sender as GridViewSelectedItemsBehavior).AssociatedObject;
                if (grid != null)
                {
                    var items = (List<MyItem>)e.NewValue;
                    grid.SelectedItems.Clear();
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            grid.SelectedItems.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        private static bool ignoreOnChanged = false;

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = AssociatedObject.SelectedItems.Cast<MyItem>().ToList();
            if (SelectedItems != items)
            {
                ignoreOnChanged = true;
                SelectedItems = items;
                ignoreOnChanged = false;
            }
        }
    }
}
