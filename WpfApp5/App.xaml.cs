using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Reactive.Bindings;
using Reactive.Bindings.Schedulers;

namespace WpfApp5
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //① ReactivePropertyの変更がUIスレッド上で発生するようにする。
            ReactivePropertyScheduler.SetDefault(new ReactivePropertyWpfScheduler(this.Dispatcher));

            //② UIスレッドで発生した未処理例外を捕獲する。
            DispatcherUnhandledException += (ss, ee) => {
                MessageBox.Show(ee.ToString());
                ee.Handled = true;
            };
        }
    }
}
