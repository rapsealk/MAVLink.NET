using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class MAVLinkController
    {
        private readonly MAVLinkModel model;
        private readonly MAVLinkView view;

        public MAVLinkController(MAVLinkModel model, MAVLinkView view)
        {
            this.model = model;
            this.view = view;
        }
    }
}
