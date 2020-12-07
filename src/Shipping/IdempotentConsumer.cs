namespace Shipping
{
    public class IdempotentConsumer
    {
        public long MessageId { get; set; }
        public string Consumer { get; set; }
    }
}