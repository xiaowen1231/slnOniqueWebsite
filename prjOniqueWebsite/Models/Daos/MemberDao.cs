using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;

namespace prjOniqueWebsite.Models.Daos
{
    public class MemberDao
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        public MemberDao(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public MemberVM GetMemberById(int id)
        {
            MemberVM vm = (from m in _context.Members
                           join c in _context.Citys
                           on m.Citys equals c.CityId
                           join a in _context.Areas
                           on m.Areas equals a.AreaId
                           join ml in _context.MemberLevel
                           on m.MemberLevel equals ml.MemberLevelId
                           where m.MemberId == id
                           select new MemberVM
                           {
                               MemberId = m.MemberId,
                               PhotoPath = m.PhotoPath,
                               Name = m.Name,
                               Password = m.Password,
                               Email = m.Email,
                               Phone = m.Phone,
                               Gender = m.Gender ? "女" : "男",
                               Citys = c.CityName,
                               Areas = a.AreaName,
                               Address = m.Address,
                               MemberLevel = ml.MemberLevelName,
                               RegisterDate = Convert.ToDateTime(m.RegisterDate).ToString("yyyy-MM-dd"),
                               DateOfBirth = Convert.ToDateTime(m.DateOfBirth).ToString("yyyy-MM-dd")
                           }).FirstOrDefault();
            return vm;
        }
        public void UpdatePhoto(Members member, MemberVM vm)
        {
            if (vm.Photo != null)
            {
                string fileName = $"MemberId_{member.MemberId}.jpg";
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
        }
        public void CreatePhoto(Members member, MemberVM vm)
        {
            if (vm.Photo != null)
            {
                string fileName = $"MemberId_{member.MemberId}.jpg";
                member.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            else
            {
                // 如果沒有上傳新照片，使用預設的照片路徑和檔名
                string defaultFileName = $"MemberId_{member.MemberId}.jpg";
                member.PhotoPath = defaultFileName;
                // 取得預設照片的完整路徑
                string defaultPhotoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", defaultFileName);
                // 複製預設照片到指定路徑
                string defaultPhotoSourcePath = Path.Combine(_environment.WebRootPath, "images", "uploads", "members", "default.jpg");
                System.IO.File.Copy(defaultPhotoSourcePath, defaultPhotoPath, true);
            }
            _context.Update(member);
            _context.SaveChanges();
        }
        public void CreateMember(MemberVM vm)
        {
            var member = new Members
            {
                Name = vm.Name,
                Password = vm.Password,
                Email = vm.Email,
                Phone = vm.Phone,
                Gender = Convert.ToBoolean(vm.Gender),
                Citys = Convert.ToInt32(vm.Citys),
                Areas = Convert.ToInt32(vm.Areas),
                Address = vm.Address,
                MemberLevel = Convert.ToInt32(vm.MemberLevel),
                RegisterDate = DateTime.Now,
                DateOfBirth = Convert.ToDateTime(vm.DateOfBirth)
            };
            _context.Update(member);
            _context.SaveChanges();
        }


    }
}
