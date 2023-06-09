﻿using System;

namespace QuanLyNhanSu.ViewModels.Account
{
    public class EditAccountViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
