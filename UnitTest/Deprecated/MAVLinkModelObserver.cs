using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class MAVLinkModelObserver : IObserver<MAVLinkModel>
    {
        //private string name;
        //private List<string> flightInfos = new List<string>();
        private IDisposable cancellation;
        private readonly string fmt = "{0,-20} {1,5}  {2, 3}";

        public MAVLinkModelObserver()
        {

        }

        public virtual void Subscribe(MAVLinkModelHandler handler)
        {
            cancellation = handler.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            //flightInfos.Clear();
        }

        public virtual void OnCompleted()
        {
            //flightInfos.Clear();
        }

        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception e)
        {
            // No implementation.
        }

        // Update information.
        public virtual void OnNext(MAVLinkModel model)
        {
            bool updated = false;


        }
    }
}
