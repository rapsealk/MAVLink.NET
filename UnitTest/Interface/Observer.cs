using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Interface
{
    public interface IMAVObserver
    {
        void Update(MAVLinkModel model);
        void UpdateArmState(bool isArmed);
    }

    public interface IMAVPublisher
    {
        void Add(IMAVObserver observer);
        void Delete(IMAVObserver observer);
        void Notify();
        void NotifyArmState(bool isArmed);
    }
}
