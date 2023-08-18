using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using static prjOniqueWebsite.Models.Infra.LineLogin;

namespace prjOniqueWebsite.Models.Daos
{
    public class MemberDao : Controller
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
            //else
            //{
            //    // 如果沒有上傳新照片，使用預設的照片路徑和檔名
            //    string defaultFileName = $"MemberId_{member.MemberId}.jpg";
            //    member.PhotoPath = defaultFileName;
            //    // 取得預設照片的完整路徑
            //    string defaultPhotoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", defaultFileName);
            //    // 複製預設照片到指定路徑
            //    string defaultPhotoSourcePath = Path.Combine(_environment.WebRootPath, "images", "uploads", "members", "default.jpg");
            //    System.IO.File.Copy(defaultPhotoSourcePath, defaultPhotoPath, true);
            //}
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
                member.PhotoPath = fileName;
                _context.SaveChanges();
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
        public List<CollectDto> GetCollects(int MemberId)
        {
            var query = (from c in _context.Collect
                         join m in _context.Members on c.MemberId equals m.MemberId into memberJoin
                         from m in memberJoin.DefaultIfEmpty()
                         join p in _context.Products on c.ProductId equals p.ProductId into productJoin
                         from p in productJoin.DefaultIfEmpty()
                         join d in _context.Discounts on p.DiscountId equals d.Id into discountJoin
                         from d in discountJoin.DefaultIfEmpty()
                         where c.MemberId == MemberId
                         select new CollectDto
                         {
                             MemberId = MemberId,
                             ProductId = p.ProductId,
                             ProductName = p.ProductName,
                             Price = p.Price,
                             PhotoPath = p.PhotoPath,
                             DiscountMethod = d.DiscountMethod,
                         });
            return query.ToList();
        }
        public List<CollectDto> CollectItems(Members member)
        {
            var collect = (from c in _context.Collect
                           join m in _context.Members on c.MemberId equals m.MemberId into memberJoin
                           from m in memberJoin.DefaultIfEmpty()
                           join p in _context.Products on c.ProductId equals p.ProductId into productJoin
                           from p in productJoin.DefaultIfEmpty()
                           join d in _context.Discounts on p.DiscountId equals d.Id into discountJoin
                           from d in discountJoin.DefaultIfEmpty()
                           where c.MemberId == member.MemberId
                           select new CollectDto
                           {
                               MemberId = member.MemberId,
                               ProductId = p.ProductId,
                               ProductName = p.ProductName,
                               Price = p.Price,
                               PhotoPath = p.PhotoPath,
                               DiscountMethod = d.DiscountMethod,
                           });
            return collect.ToList();
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

        public Members BindLineLogin(LineRegisterVM vm, Infra.LineLogin.LineProfile lineProfile)
        {
            var member = _context.Members.FirstOrDefault(m => m.Email == vm.Email);

            member.Name = lineProfile.displayName;
            member.Password = vm.Password;
            member.Phone = vm.Phone;
            member.Email = vm.Email;
            member.Gender = Convert.ToBoolean(vm.Gender);
            member.Citys = Convert.ToInt32(vm.Citys);
            member.Areas = Convert.ToInt32(vm.Areas);
            member.Address = vm.Address;
            member.MemberLevel = 1;
            member.LineUserId = lineProfile.userId;
            member.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
            if (lineProfile.pictureUrl != null)
            {
                member.PhotoPath = $"MemberId_{member.MemberId}.jpg";
                CopyImage(lineProfile.pictureUrl, member.PhotoPath);
            }
            _context.SaveChanges();
            return member;
        }
        public async void CopyImage(string pictureUrl,string fileName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imgByte = await client.GetByteArrayAsync(pictureUrl);

                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);

                    await System.IO.File.WriteAllBytesAsync(photoPath, imgByte);
                }
            }catch (Exception ex)
            {
                throw new Exception("儲存照片失敗");
            }
            
        }

        public Members LineRegister(LineRegisterVM vm, Infra.LineLogin.LineProfile lineProfile)
        {

            var member = new Members
            {
                Name = lineProfile.displayName,
                Password = vm.Password,
                Phone = vm.Phone,
                Email = vm.Email,
                Gender = Convert.ToBoolean(vm.Gender),
                Citys = Convert.ToInt32(vm.Citys),
                Areas = Convert.ToInt32(vm.Areas),
                Address = vm.Address,
                MemberLevel = 1,
                DateOfBirth = Convert.ToDateTime(vm.DateOfBirth),
                RegisterDate = DateTime.Now,
                LineUserId = lineProfile.userId
            };

            _context.Members.Add(member);
            _context.SaveChanges();

            if (lineProfile.pictureUrl != null)
            {
                string fileName = $"MemberId_{member.MemberId}.jpg";
                CopyImage(lineProfile.pictureUrl, fileName);

                member.PhotoPath = fileName;

                _context.SaveChanges();
            }

            return member;
        }
    }
}


