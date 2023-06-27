using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Eshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }

        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }

        [DisplayName("Còn hoạt động")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ Invoice
        //public List<Invoice> Invoices { get; set; }

        // Collection reference property cho khóa ngoại từ Cart
        //public List<Cart> Carts { get; set; }
        //public List<News> Newss { get; set; }
    }
}
