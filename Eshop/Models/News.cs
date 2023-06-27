using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Models
{
    public class News
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        // Navigation reference property cho khóa ngoại đến Account 
        //[DisplayName("Nhân viên")]
        //public Account Account { get; set; }
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [DisplayName("Tóm tắc")]
        public string Summary { get; set; }
        [DisplayName("Nội dung")]
        public string Content { get; set; }
        [DisplayName("Hình ảnh")]
        public string Image { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; } = DateTime.Now;
        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

    }
}
