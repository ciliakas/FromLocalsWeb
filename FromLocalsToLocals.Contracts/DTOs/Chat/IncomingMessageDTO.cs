namespace FromLocalsToLocals.Contracts.DTO
{
    public class IncomingMessageDTO
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactId { get; set; }
    }
}