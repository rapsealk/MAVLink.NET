using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    // Observer Pattern (https://docs.microsoft.com/ko-kr/dotnet/standard/events/observer-design-pattern)
    class MAVLinkModelHandler
    {
        private List<IObserver<MAVLinkModel>> observers;
        private readonly List<MAVLinkModel> models;

        public MAVLinkModelHandler()
        {
            observers = new List<IObserver<MAVLinkModel>>();
            models = new List<MAVLinkModel>();
        }

        public IDisposable Subscribe(IObserver<MAVLinkModel> observer)
        {
            // Check whether observer is already registered. If not, add it.
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                // (https://docs.microsoft.com/ko-kr/dotnet/api/system.iobserver-1.onnext?view=netframework-4.7.2)
                foreach (var model in models)
                    observer.OnNext(model);
            }
            return new Unsubscriber<MAVLinkModel>(observers, observer);
        }

        /* Called to indicate all baggages is now unloaded.
        public void BaggageStatus(int flightNo, string from, int carousel)
        {
            var info = new BaggageInfo(flightNo, from, carousel);

            // Carousel is assigned, so add new info object to list.
            if (carousel > 0 && !flights.Contains(info))
            {
                flights.Add(info);
                foreach (var observer in observers)
                    observer.OnNext(info);
            }
            else if (carousel == 0)
            {
                // Baggage claim for flight is done.
                var flightsToRemove = new List<BaggageInfo>();
                foreach (var flight in flights)
                {
                    if (info.FlightNumber == flight.FlightNumber)
                    {
                        flightsToRemove.Add(flight);
                        foreach (var observer in observers)
                            observer.OnNext(info);
                    }
                }
                foreach (var flightToRemove in flightsToRemove)
                    flights.Remove(flightToRemove);

                flightsToRemove.Clear();
            }
        }

        public void LastBaggageClaimed()
        {
            foreach (var observer in observers)
                observer.OnCompleted();

            observers.Clear();
        }
        */

        internal class Unsubscriber<MAVLinkModel> : IDisposable
        {
            private List<IObserver<MAVLinkModel>> _observers;
            private readonly IObserver<MAVLinkModel> _observer;

            internal Unsubscriber(List<IObserver<MAVLinkModel>> observers, IObserver<MAVLinkModel> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
