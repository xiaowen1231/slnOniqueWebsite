﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjOniqueWebsite.Models.EFModels
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int MethodId { get; set; }
        public string ShippingAddress { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int PaymentMethodId { get; set; }

        public virtual Members Member { get; set; }
        public virtual ShippingMethods Method { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PaymentMethods PaymentMethod { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}