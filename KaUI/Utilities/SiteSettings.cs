﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KaUI.Utilities
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public string UrlApi { get; set; }
        public string WWWroot { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }

    }

    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumic { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Encryptkey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
    }

    public class EventBus
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
