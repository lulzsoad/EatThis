﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI
{
    public class NoReplyEmailSettings
    {
        public string HostSmtp { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string SenderEmailPassword { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
