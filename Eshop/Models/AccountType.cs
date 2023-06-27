//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//namespace Eshop.Models
//{
//    public class AccountType
//    {
//        public int Id { get; set; }
//        [DisplayName("Loại sản phẩm")]
//        [Required(ErrorMessage = "{0} không được bỏ trống")]
//        public string Name { get; set; }

//        [DisplayName("Còn hiệu lực")]
//        [DefaultValue(true)]
//        public bool Status { get; set; } = true;
//        public List<Account> Accounts { get; set; }
//    }
//}
