namespace HMSDataRepo.Model
{
    public class SeverResponceMessage
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public int? InsertedID { get; set; }
        public object Data { get; set; }
    }
}
