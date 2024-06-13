using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ODT_System.DTO;
using ODT_System.Enums;
using ODT_System.Helpers;
using ODT_System.Models;
using ODT_System.Repository.Interface;
using ODT_System.Services.Interface;
using ODT_System.SharedObject;
using ODT_System.Utils.Interface;

namespace ODT_System.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IStudyTimeRepository _studyTimeRepository;
        private readonly IMailHandler _mailHandler;
        private readonly IBcryptHandler _bcryptHandler;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public AccountService(IUserRepository userRepository, IMailHandler mailHandler,
            IBcryptHandler bcryptHandler, IMapper mapper, IMemoryCache cache, IPostRepository postRepository,
            IStudyTimeRepository studyTimeRepository)
        {
            _userRepository = userRepository;
            _mailHandler = mailHandler;
            _bcryptHandler = bcryptHandler;
            _mapper = mapper;
            _cache = cache;
            _postRepository = postRepository;
            _studyTimeRepository = studyTimeRepository;
        }

        public bool VerifyEmail(VerifyEmailDTO verifyEmailDTO, out string message)
        {
            var email = verifyEmailDTO.Email;

            // Find user by email
            var user = _userRepository.FindByEmail(email);

            // Check if user is not found
            if (user == null)
            {
                message = "Email không tồn tại";
                return false;
            }

            //Generate OTP by random number contains 6 digits
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            string subject = "Mã OTP để lấy lại mật khẩu";
            string body = "Mã OTP để lấy lại mật khẩu của bạn là: <b>" + otp + "</b>"
                + $"<br>Mã OTP có hiệu lực trong vòng 1 phút!"
                + $"<br>Vui lòng nhập mã OTP này để lấy lại mật khẩu!"
                + $"<br><strong>Vui lòng không tắt ứng dụng trong lúc lấy lại mật khẩu!<strong>"
                + $"<br><br>Trân trọng.<br>";

            //Send OTP to email
            _mailHandler.Send(email,
                    subject,
                    body);

            // Set time expire cache for OTP
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            // Store OTP to cache
            _cache.Set(email, otp.ToString(), cacheEntryOptions);

            message = "OTP đã được gửi";
            return true;

        }

        public bool NewPassword(NewPasswordDTO newPasswordDTO, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(newPasswordDTO.Email);

            if (user == null)
            {
                message = "Email không tồn tại";
                return false;
            }

            var isOtpExist = _cache.TryGetValue(newPasswordDTO.Email, out string otp);

            // Check OTP
            if (!isOtpExist)
            {
                message = "OTP không còn tồn tại!";
                return false;
            }
            else if (otp != newPasswordDTO.OTP)
            {
                message = "OTP không chính xác!";
                return false;
            }

            // Hash new password
            user.Password = _bcryptHandler.HashPassword(newPasswordDTO.Password);

            // Update new password
            _userRepository.Update(user);
            _userRepository.Save();

            message = "Đổi mật khẩu thành công";
            return true;
        }

        public ViewProfileDTO? FindUserProfile(string email)
        {
            var user = _userRepository.FindByEmail(email);
            var profileDTO = _mapper.Map<ViewProfileDTO>(user);
            return profileDTO;
        }

        public bool UpdateProfile(UpdateProfileDTO updateProfileDTO, string emailAccount)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(emailAccount);

            if (user == null)
            {
                return false;
            }

            user.FullName = updateProfileDTO.FullName == null ? user.FullName : updateProfileDTO.FullName;
            user.Phone = updateProfileDTO.Phone == null ? user.Phone : updateProfileDTO.Phone;
            user.Gender = updateProfileDTO.Gender == null ? user.Gender : updateProfileDTO.Gender.Value;
            user.Dob = updateProfileDTO.Dob == null ? user.Dob : updateProfileDTO.Dob.Value;
            user.Desciption = updateProfileDTO.Desciption == null ? user.Desciption : updateProfileDTO.Desciption;

            _userRepository.Update(user);
            _userRepository.Save();
            return true;
        }

        public bool ChangePassword(ChangePasswordDTO changePasswordDTO, string email, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmail(email);

            // Check if user is not found
            if (user == null)
            {
                message = "Tài khoản không tồn tại";
                return false;
            }

            // Check if old password is correct
            if (!_bcryptHandler.VerifyPassword(changePasswordDTO.OldPassword, user.Password))
            {
                message = "Mật khẩu cũ không chính xác";
                return false;
            }

            // Hash new password
            user.Password = _bcryptHandler.HashPassword(changePasswordDTO.NewPassword);

            // Update new password
            _userRepository.Update(user);
            _userRepository.Save();
            message = "Cập nhật mật khẩu thành công";
            return true;
        }

        public bool CreatePost(PostCreateDTO postCreateDTO, string userEmail, out string message)
        {
            // Find user by id
            var user = _userRepository.FindByEmailIncludeRole(userEmail);

            if (user == null)
            {
                message = "Tài khoản không tồn tại";
                return false;
            }
            else if (user.Role.Name != RoleEnum.Tutor.ToString())
            {
                message = "Tài khoản không phải là gia sư. Không thể tạo bài viết";
                return false;
            }

            // Map postCreateDTO to Post
            var post = _mapper.Map<Post>(postCreateDTO);

            // Set user to post
            post.UserId = user.Id;
            post.Status = (int)PostStatusEnum.Pending;
            post.IsDeleted = false;
            post.IsHidden = false;
            post.CreatedAt = DateTime.Now;

            // Add post to database
            _postRepository.Add(post);
            _postRepository.Save();

            message = "Tạo bài đăng thành công";
            return true;
        }

        public PaginatedModel<PostTutorDTO> ListPost(string userEmail, int? pageIndex, int? pageSize, int? status, string? textSearch)
        {
            // Find user by email
            var user = _userRepository.FindByEmailIncludeRole(userEmail);

            if (user == null)
            {
                return null;
            }

            // Get list post by user id
            var posts = _postRepository.GetAll().Where(p => p.UserId == user.Id && p.IsDeleted == false);

            // Filter by status
            if (status != null)
            {
                posts = posts.Where(p => p.Status == status);
            }

            // Filter by text search
            if (!string.IsNullOrEmpty(textSearch))
            {
                var pattern = $"%{textSearch}%";
                posts = posts.Where(p => EF.Functions.Like(p.ShortDescription, pattern)
                                         || EF.Functions.Like(p.Subject, pattern)
                                         || EF.Functions.Like(p.ContactPhone, pattern)
                                         || EF.Functions.Like(p.StudyAddress, pattern));
            }

            // Include other properties
            posts = posts.Include(p => p.StudyTimes);

            // Paging
            var paginatedModel = PaginatedModel<Post>.GetPaging(pageIndex, pageSize, posts);

            // Map Post to PostDTO
            var postDTOs = _mapper.Map<List<PostTutorDTO>>(paginatedModel.items);
            var paginatedModelDTO = new PaginatedModel<PostTutorDTO>
            {
                pageIndex = paginatedModel.pageIndex,
                pageSize = paginatedModel.pageSize,
                totalItems = paginatedModel.totalItems,
                items = postDTOs
            };

            return paginatedModelDTO;
        }

        public PostTutorDTO? GetPostById(int id, string userEmail)
        {
            // Find user by email
            var user = _userRepository.FindByEmailIncludeRole(userEmail);

            if (user == null)
            {
                return null;
            }

            // Find post by id
            var post = _postRepository.FindByIdIncludeStudyTimes(id);

            // Map to PostTutorDTO
            var postDTO = _mapper.Map<PostTutorDTO>(post);

            return postDTO;
        }

        public bool UpdatePost(PostUpdateDTO postUpdateDTO, string userEmail, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmailIncludeRole(userEmail);

            if (user == null)
            {
                message = "Có lỗi trong quá trình xác thực tài khoản";
                return false;
            }

            // Find post by id
            var post = _postRepository.FindByIdIncludeStudyTimes(postUpdateDTO.Id);
            if (post == null || post.UserId != user.Id)
            {
                message = "Không tìm thấy bài viết hoặc bạn không có quyền chỉnh sửa bài viết này";
                return false;
            }

            // Map StudyTimeUpdateDTO to StudyTime
            var studyTimesUpdate = _mapper.Map<List<StudyTime>>(postUpdateDTO.StudyTimes);

            // Add post id to study time
            foreach (var studyTime in studyTimesUpdate)
            {
                studyTime.PostId = post.Id;
            }

            // Map postUpdateDTO to post
            post.ContactPhone = postUpdateDTO.ContactPhone;
            post.ShortDescription = postUpdateDTO.ShortDescription;
            post.StudyAddress = postUpdateDTO.StudyAddress;
            post.NumberOfStudent = postUpdateDTO.NumberOfStudent;
            post.StartDate = postUpdateDTO.StartDate;
            post.StudyHour = postUpdateDTO.StudyHour;
            post.Subject = postUpdateDTO.Subject;
            post.StudentGender = postUpdateDTO.StudentGender;
            post.Fee = postUpdateDTO.Fee;
            post.TypeOfFee = postUpdateDTO.TypeOfFee;
            post.DayPerWeek = postUpdateDTO.DayPerWeek;
            post.Description = postUpdateDTO.Description;
            post.Status = (int)PostStatusEnum.Pending;
            post.UpdatedAt = DateTime.Now;

            // Delete all study time of post
            foreach (var studyTime in post.StudyTimes)
            {
                _studyTimeRepository.Delete(studyTime);
            }

            // Add new study time to db
            foreach (var studyTime in studyTimesUpdate)
            {
                _studyTimeRepository.Add(studyTime);
            }

            // Update post
            _postRepository.Update(post);
            _postRepository.Save();

            message = "Cập nhật bài viết thành công";
            return true;
        }

        public bool DeletePost(int id, string userEmail, out string message)
        {
            // Find user by email
            var user = _userRepository.FindByEmailIncludeRole(userEmail);

            if (user == null)
            {
                message = "Có lỗi trong quá trình xác thực tài khoản";
                return false;
            }

            // Find post by id
            var post = _postRepository.Find(id);

            if (post == null || post.UserId != user.Id)
            {
                message = "Không tìm thấy bài viết hoặc bạn không có quyền xóa bài viết này";
                return false;
            }

            // Set isDeleted to true
            post.IsDeleted = true;

            // Update post
            _postRepository.Update(post);
            _postRepository.Save();

            message = "Xóa bài viết thành công";
            return true;
        }
    }
}
