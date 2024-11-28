﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Models
{
    public class EmailSettings
    {
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderUsername { get; set; }
        public string? SenderPassword { get; set; }
        public string? SenderName { get; set; }
    }
}
