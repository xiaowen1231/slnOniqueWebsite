using Microsoft.AspNetCore.Components.Forms;
using prjOniqueWebsite.Models.DTOs;
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
        public List<MemberListDto> GetMembers()
        {
            var query = from m in _context.Members
                        join ml in _context.MemberLevel
                        on m.MemberLevel equals ml.MemberLevelId
                        select new MemberListDto
                        {
                            MemberId = m.MemberId,
                            PhotoPath = m.PhotoPath,
                            Name = m.Name,
                            Phone = m.Phone,
                            Email = m.Email,
                            DateOfBirth = m.DateOfBirth,
                            Gender = m.Gender ? "女" : "男",
                            MemberLevelName = ml.MemberLevelName,
                        };
            return query.ToList();
        }
        public Members GetMemberByEmail(string email)
        {
            var memInDb = _context.Members.FirstOrDefault(m => m.Email == email);
            if (memInDb == null)
            {
                return null;
            }
            return memInDb;
        }
        public Members GetMemberByPhone(string phone)
        {
            var memInDb = _context.Members.FirstOrDefault(m => m.Phone == phone);
            if (memInDb == null)
            {
                return null;
            }
            return memInDb;
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
            _context.Members.Add(member);
            _context.SaveChanges();
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
        public MemberEditDto GetMemberById(int id)
        {
            MemberEditDto dto = (from m in _context.Members
                                 join c in _context.Citys
                                 on m.Citys equals c.CityId
                                 join a in _context.Areas
                                 on m.Areas equals a.AreaId
                                 join ml in _context.MemberLevel
                                 on m.MemberLevel equals ml.MemberLevelId
                                 where m.MemberId == id
                                 select new MemberEditDto
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
            return dto;
        }
        public void EditMember(MemberEditVM vm)
        {
            var member = _context.Members.Where(m => m.MemberId == vm.MemberId).FirstOrDefault();
            if (vm.Photo != null)
            {
                string fileName = $"MemberId_{member.MemberId}.jpg";
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            member.Name = vm.Name;
            member.Password = vm.Password;
            member.Phone = vm.Phone;
            member.Email = vm.Email;
            member.Citys = Convert.ToInt32(vm.Citys);
            member.Areas = Convert.ToInt32(vm.Areas);
            member.Address = vm.Address;
            member.MemberLevel = Convert.ToInt32(vm.MemberLevel);
            _context.SaveChanges();
        }
        public void Register(FMemberVM vm)
        {
            var mem = new Members
            {
                Name = vm.Name,
                Password = vm.Password,
                Email = vm.Email,
                Phone = vm.Phone,
                Gender = Convert.ToBoolean(vm.Gender),
                Citys = Convert.ToInt32(vm.Citys),
                Areas = Convert.ToInt32(vm.Areas),
                Address = vm.Address,
                MemberLevel = 1,
                RegisterDate = DateTime.Now,
                DateOfBirth = Convert.ToDateTime(vm.DateOfBirth)
            };
            _context.Members.Add(mem);
            _context.SaveChanges();
            if (vm.Photo != null)
            {
                string fileName = $"MemberId_{mem.MemberId}.jpg";
                mem.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            else
            {
                // 如果沒有上傳新照片，使用預設的照片路徑和檔名
                string defaultFileName = $"MemberId_{mem.MemberId}.jpg";
                mem.PhotoPath = defaultFileName;
                // 取得預設照片的完整路徑
                string defaultPhotoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", defaultFileName);
                // 複製預設照片到指定路徑
                string defaultPhotoSourcePath = Path.Combine(_environment.WebRootPath, "images", "uploads", "members", "default.jpg");
                System.IO.File.Copy(defaultPhotoSourcePath, defaultPhotoPath, true);
            }
            _context.Update(mem);
            _context.SaveChanges();
        }
        public FMemberEditDto GetFMemberById(int id)
        {
            FMemberEditDto mem = (from m in _context.Members
                                  join c in _context.Citys
                                  on m.Citys equals c.CityId
                                  join a in _context.Areas
                                  on m.Areas equals a.AreaId
                                  where m.MemberId == id
                                  select new FMemberEditDto
                                  {
                                      MemberId = m.MemberId,
                                      Name = m.Name,
                                      DateOfBirth = Convert.ToDateTime(m.DateOfBirth).ToString("yyyy-MM-dd"),
                                      Email = m.Email,
                                      Phone = m.Phone,
                                      Gender = m.Gender ? "女" : "男",
                                      Citys = c.CityName,
                                      Areas = a.AreaName,
                                      Address = m.Address
                                  }).FirstOrDefault();
            return mem;
        }
        public void EditFMember(FMemberEditVM vm)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberId == vm.MemberId);

            member.Name = vm.Name;
            member.Phone = vm.Phone;
            member.Email = vm.Email;
            member.Citys = Convert.ToInt32(vm.Citys);
            member.Areas = Convert.ToInt32(vm.Areas);
            member.Address = vm.Address;
            _context.SaveChanges();
        }
        public void FMemberPassword(FMemberPasswordVM vm, int loginMemId)
        {
            var memberInDb = _context.Members.FirstOrDefault(m => m.MemberId == loginMemId);
            memberInDb.Password = vm.NewPassword;
            _context.SaveChanges();
        }
        public List<MemberOrderDto> GetMemberOrders(int MemberId)
        {

            var memberorder = (from m in _context.Members
                               join o in _context.Orders
                               on m.MemberId equals o.MemberId
                               join p in _context.PaymentMethods
                               on o.PaymentMethodId equals p.PaymentMethodId
                               where m.MemberId == MemberId

                               select (new MemberOrderDto
                               {
                                   OrderId = o.OrderId,
                                   OrderDate = o.OrderDate,
                                   PaymentMethodName = p.PaymentMethodName,
                                   TotalPrice = o.TotalPrice
                               })).OrderByDescending(o => o.OrderDate).ToList();


            return memberorder;

        }

    }
}


