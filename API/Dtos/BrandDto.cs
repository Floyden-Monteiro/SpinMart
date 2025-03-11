using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BrandToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }

    public class BrandUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}