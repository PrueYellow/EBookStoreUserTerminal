﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models.EFModels
{
    public partial class Users
    {
        public Users()
        {
            Carts = new HashSet<Carts>();
            Comments = new HashSet<Comments>();
            EbookOrders = new HashSet<EbookOrders>();
            Orders = new HashSet<Orders>();
            UserArticleCollections = new HashSet<UserArticleCollections>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmCode { get; set; }

        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<EbookOrders> EbookOrders { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<UserArticleCollections> UserArticleCollections { get; set; }
    }
}