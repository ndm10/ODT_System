using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ODT_System.DTO;
using ODT_System.Enums;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;

namespace ODT_System.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public bool CreateFeedback(FeedbackCreateDTO feedbackDTO, string email, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmailIncludeRole(email);

            if (user == null)
            {
                message = "Có lỗi trong quá trình xác thực tài khoản";
                return false;
            }

            // Map feedbackDTO to Feedback entity
            var feedback = _mapper.Map<Feedback>(feedbackDTO);

            feedback.IsDelete = false;
            feedback.CreateAt = DateTime.Now;
            feedback.CreateById = user.Id;
            feedback.Status = FeedBackStatusEnum.Display.ToString();

            // Create feedback
            _feedbackRepository.Add(feedback);

            // Save changes
            _feedbackRepository.Save();

            message = "Tạo feed back thành công";
            return true;
        }

        public List<FeedbackCommonDTO> GetFeedbacksById(int id)
        {
            var feedbacks = _feedbackRepository.GetAll()
                .Where(x => x.FeedbeckToId == id 
                        && x.Status == FeedBackStatusEnum.Display.ToString()
                        && x.IsDelete == false)
                .Include(x => x.CreateBy)
                .ToList();

            return _mapper.Map<List<FeedbackCommonDTO>>(feedbacks);
        }
    }
}
