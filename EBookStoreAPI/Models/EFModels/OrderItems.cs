﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models.EFModels
{
    public partial class OrderItems
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        public virtual Books Book { get; set; }
        public virtual Orders Order { get; set; }
    }
}