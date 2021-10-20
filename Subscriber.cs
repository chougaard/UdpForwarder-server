namespace udpForwarder
{
    public class Subscriber
    {
        public IPublisher Publisher { get; set; }
        public Subscriber(IPublisher publisher)
        {
            Publisher = publisher;
        }
    }
}