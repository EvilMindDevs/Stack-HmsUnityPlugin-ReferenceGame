namespace GameDev.Library
{
    public interface IEvent
    {
        string EventName { get; set; }
    };

    public class GEvent<TEventData> : IEvent
    {
        private string _eventName;
        private TEventData _data;

        public GEvent(string eventName, TEventData data)
        {
            _eventName = eventName;
            _data = data;
        }

        public string EventName
        {
            get { return _eventName; }
            set { _eventName = value; }
        }

        public TEventData Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public override string ToString()
        {
            return "Event with type of data " + typeof(TEventData).Name;
        }
    }
}


