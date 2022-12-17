using MAVLink.NET.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVLink.NET.Presenters
{
    public class MainPresenter
    {
        IMainView view;

        public MainPresenter(IMainView view)
        {
            this.view = view;
        }
    }
}
