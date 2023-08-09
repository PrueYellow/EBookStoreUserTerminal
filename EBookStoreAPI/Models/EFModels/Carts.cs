﻿#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models
{
    public partial class Carts
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Qty { get; set; }

        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
    }
}