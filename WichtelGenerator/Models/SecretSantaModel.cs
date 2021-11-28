using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WichtelGenerator.Core.Models
{
    public class SecretSantaModel
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }
        public string MailAdress { get; set; }
        public SecretSantaModel Choise { get; set; }
    }

    public class ListMapping
    {
        [Key]
        public Guid Id { get; set; }

        public SecretSantaModel Owner { get; set; }
        public SecretSantaModel SecretSanta { get; set; }

        public SecredSantaMappingType MappingType { get; set; }
    }

    public enum SecredSantaMappingType
    {
        WhiteListed,
        BlackListed
    }
}