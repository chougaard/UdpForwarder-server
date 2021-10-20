namespace udpForwarder
{
    public class Message
    {
        public byte[] Content { get; set; }
        public int ContentLength {get;set;}
        public Message(byte[] _content, int length = 0)
        {
            Content = _content;
            ContentLength = length;
        }
    }
}