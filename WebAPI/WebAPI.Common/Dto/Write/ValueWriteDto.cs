using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Dto.Write
{
    public class ValueWriteDto
    {
        [Required]
        [StringLength(10, ErrorMessage = "The maximum name length can be 10.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "The maximum description length can be 50.")]
        public string Description { get; set; }

        public IEnumerable<ChildWriteDto> Children { get; set; }
    }
}
