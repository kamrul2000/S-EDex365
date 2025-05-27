namespace S_EDex365.API.Models
{
    public class SolutionChatResponse
    {
        //public Guid UserId { get; set; } // Added to represent each user's chat group
        public string UserType { get; set; }
        public List<ClaimCommunicationResponse> Chats { get; set; }
    }
}
