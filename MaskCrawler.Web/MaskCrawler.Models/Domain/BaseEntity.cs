using System.ComponentModel.DataAnnotations;

namespace MaskCrawler.Models.Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
