﻿using System;

namespace aes.Models
{
    public class OdsEdit
    {
        public int Id { get; set; }
        public Ods Ods { get; set; }
        public string EditingByUserId { get; set; }
        public DateTime EditTime { get; set; }
    }
}
