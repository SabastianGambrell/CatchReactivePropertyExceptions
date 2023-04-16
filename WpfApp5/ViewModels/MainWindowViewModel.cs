using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.ViewModels
{
    public class MainWindowViewModel : Bases.ViewModelBase
    {
        public List<MyItem> MyItems { get; set; } = Enumerable.Range(0, 10).Select(a => new MyItem(Guid.NewGuid().ToString())).ToList();
        public ReactiveProperty<List<MyItem>> SelectedItems { get; set; } = new ReactiveProperty<List<MyItem>>();
        public ReactiveProperty<bool> IsException { get; set; } = new ReactiveProperty<bool>(true);

        public ReactiveCommand TestCommand { get; set; }

        public MainWindowViewModel()
        {
            // BehaviorからではなくコマンドからSelectedItemsを更新すると、DispatcherUnhandledException で捕獲できる。
            TestCommand = new ReactiveCommand().WithSubscribe(() => {
                SelectedItems.Value = 
                    SelectedItems.Value == null ? 
                        new List<MyItem>() : 
                        SelectedItems.Value.Select(a => a).ToList();
            }).AddTo(disposables);

            // Behaviorから更新されると、DispatcherUnhandledException で捕獲できない。
            SelectedItems.Where(a => a != null).Subscribe(a =>
            {
                if (IsException.Value) throw new ApplicationException();
                else a.ForEach(b => System.Console.WriteLine(b.Name));
            }).AddTo(disposables);

            // ※ Behaviorから更新されても、ObserveOnUIDispatcher を実行すると、集約例外で捕獲できる。
            //SelectedItems.Where(a => a != null).ObserveOnUIDispatcher().Subscribe(a =>
            //{
            //    if (IsException.Value) throw new ApplicationException();
            //    else a.ForEach(b => System.Console.WriteLine(b.Name));
            //}).AddTo(disposables);

            // ※ Behaviorから更新されても、asyncメソッドにすると、集約例外で捕獲できる。
            //SelectedItems.Where(a => a != null).Subscribe(async a =>
            //{
            //    await Task.CompletedTask;
            //    if (IsException.Value) throw new ApplicationException();
            //    else a.ForEach(b => System.Console.WriteLine(b.Name));
            //}).AddTo(disposables);
        }
    }
}
