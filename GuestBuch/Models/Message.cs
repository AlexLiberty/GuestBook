namespace GuestBook.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string? MessageText {get; set; }
        public string? Timestamp { get; set; }
        public User User { get; set; }

        public Message() 
        {
            Timestamp = DateTime.Now.ToString("f");
        }
    }
}
