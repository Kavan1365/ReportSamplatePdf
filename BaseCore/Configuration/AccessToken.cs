﻿using System;

namespace BaseCore.Configuration
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string expires { get; set; }
        public Guid Guid { get; set; }
        public int id { get; set; }

    }
}
