﻿using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class ItemDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public ItemType ItemType { get; set; }

        public PaymentType PaymentType { get; set; }

        public int Cost { get; set; }

        public ItemDTO(int id, string name, string picture, ItemType itemType, PaymentType paymentType, int cost)
        {
            Id = id;
            Name = name;
            Picture = picture;
            ItemType = itemType;
            PaymentType = paymentType;
            Cost = cost;
        }
    }
}
