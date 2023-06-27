//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;
//namespace Eshop.Models
//{
//    public class ProductDetail
//    {
//        public int Id { get; set; }
//        public int ProductId { get; set; }
//        // Navigation reference property cho khóa ngoại đến Product
//        [DisplayName("Sản phẩm")]
//        public Product Product { get; set; }
//        [DisplayName("GPU")]
//        public string GPU { get; set; }
//        [DisplayName("CPU")]
//        public string CPU { get; set; }
//        [DisplayName("RAM")]
//        public string RAM { get; set; }
//        [DisplayName("ROM")]
//        public string HardDrive { get; set; }
//        [DisplayName("Màn hình")]
//        public string Screens { get; set; }
//        [DisplayName("Pin")]
//        public string Battery { get; set; }
//        [DisplayName("Kích thước")]
//        public string Size { get; set; }
//        [DisplayName("Trọng lượng")]
//        public string Weight { get; set; }
//        [DisplayName("Cổng kết nối")]
//        public string Connector { get; set; }
//        [DisplayName("Tiện ích khác")]
//        public string OtherUtilities { get; set; }

//    }
//}
