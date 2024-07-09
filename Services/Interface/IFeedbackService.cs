using ODT_System.DTO;

namespace ODT_System.Services.Interface
{
    public interface IFeedbackService
    {
        public bool CreateFeedback(FeedbackCreateDTO feedbackDTO, string email, out string message1);
        public List<FeedbackCommonDTO> GetFeedbacksById(int id);
    }
}
