namespace FromLocalsToLocals.Contracts.DTO
{
    public class OutGoingMessageDTO
    {
        public string Message { get; set; }
        public bool IsUserTab { get; set; }
        public int ContactID { get; set; }
        public byte[] Image { get; set; }
        public string VendorTitle { get; set; }
        public string UserToSendId { get; set; }
    }
}