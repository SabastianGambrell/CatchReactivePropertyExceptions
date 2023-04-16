using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.ViewModels
{
    public class MyItem : Bases.ViewModelBase
    {
        public string Name { get; set; }

        public MyItem(string name)
        {
            Name = name;
        }
    }
}
