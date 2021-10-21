using System;


namespace udpForwarder
{
    public interface IPublisher
    {
        event EventHandler<Message> Handler;
        void Publish(byte[] cont, int length);
    }

    public class Publisher : IPublisher
    {
        public event EventHandler<Message> Handler;

        public void OnPublish(Message msg)
        {
            Handler?.Invoke(this, msg);
        }

        public void Publish(byte[] cont, int length)
        {
            Message msg = new Message(cont, length);
            // Message msg = (Message)Activator.CreateInstance(typeof(Message), cont);
            OnPublish(msg);
        }
    }
}