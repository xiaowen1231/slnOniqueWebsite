using prjOniqueWebsite.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace prjOniqueWebsite.Models.ViewModels
{
    public class BgProductsPagingVM
    {
        public List<Products> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrenPage { get; set; }
        public int TotalPages {get; set; }
    }
   
}
