using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class ProductPromotion
    {
        public int Id { get; set; }

        [DisplayName("Phần trăm khuyến mãi")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public int Percent { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Ngày lập")]
        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; } = DateTime.Now;
        [DisplayName("Ngày hết hạn")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public DateTime ExpirationDate { get; set; }

        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ Product
        //public List<Product> Products { get; set; }
    }
}
