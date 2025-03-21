﻿using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModel
{
    public class LoginVM
    {
        #region Properties
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? RedirectUrl { get; set; }
        #endregion
    }
}
