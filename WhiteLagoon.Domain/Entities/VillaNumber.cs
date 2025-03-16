using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        #region Properties

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Villa Number")]
        public int Villa_Number { get; set; }

        [Display(Name = "Villa Id")]
        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        [ValidateNever] // to ignore it from validtion to complete the Create process
        public Villa Villa { get; set; }

        [Display(Name = "Special Details")]
        public string? SpecialDetails { get; set; }

        #endregion
    }
}
