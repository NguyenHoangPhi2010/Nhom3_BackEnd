using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string SKU { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Giá (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Price { get; set; } = 0;

        [DisplayName("Tồn kho")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;
        [DisplayName("Dánh giá")]
        [DefaultValue(0)]
        public int Rating { get; set; } = 0;
        [DisplayName("GPU")]
        public string GPU { get; set; }
        [DisplayName("CPU")]
        public string CPU { get; set; }
        [DisplayName("RAM")]
        public string RAM { get; set; }
        [DisplayName("ROM")]
        public string HardDrive { get; set; }
        [DisplayName("Màn hình")]
        public string Screens { get; set; }
        [DisplayName("Pin")]
        public string Battery { get; set; }
        [DisplayName("Kích thước")]
        public string Size { get; set; }
        [DisplayName("Trọng lượng")]
        public string Weight { get; set; }
        [DisplayName("Cổng kết nối")]
        public string Connector { get; set; }
        [DisplayName("Tiện ích khác")]
        public string OtherUtilities { get; set; }
        public int ProductTypeId { get; set; }

        // Navigation reference property cho khóa ngoại đến ProductType
        [DisplayName("Loại sản phẩm")]
        public ProductType ProductType { get; set; }
        public int ProductPromotionId { get; set; }
        [DisplayName("Khuyến mãi")]
        public ProductPromotion ProductPromotion { get; set; }
        [DisplayName("Ảnh minh họa")]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        [DisplayName("Sản phẩm hot")]
        [DefaultValue(false)]
        public bool ProductHot { get; set; } = false;
        [DisplayName("Sản phẩm mới")]
        [DefaultValue(false)]
        public bool ProductNew { get; set; } = false;

        // Collection reference property cho khóa ngoại từ Cart
        //public List<Cart> Carts { get; set; }
        // Collection reference property cho khóa ngoại từ InvoiceDetail
        //public List<InvoiceDetail> InvoiceDetails { get; set; }
        // Collection reference property cho khóa ngoại từ ProductDetail
        //public List<ProductDetail> ProductDetail { get; set; }
        // Collection reference property cho khóa ngoại từ ProductDetail
        // public List<ProductImage> ProductImage { get; set; }
    }
}
