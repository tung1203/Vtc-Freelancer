using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vtc_Freelancer.Models
{
    public class Languages
    {
        public int LanguageId { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Language { get; set; }
        public int Level { get; set; }
        public int SellerId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Seller Seller { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
        public Languages() { }

    }
}