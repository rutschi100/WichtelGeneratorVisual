using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Konsole.Test
{
    public class SecretSantaModel
    {
        [Key]
        public Guid ID { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string MailAdress { get; set; }
        
        public SecretSantaModel Choise { get; set; }

        public List<SantaBlackList> BlackList { get; } = new List<SantaBlackList>();
        public List<SantaWhiteList> WhiteList { get; } = new List<SantaWhiteList>();
    }

    public class SantaBlackList
    {
        [Key]
        public Guid ID { get; set; }

        public List<SecretSantaModel> BlackList { get; set; }
    }

    public class SantaWhiteList
    {
        [Key]
        public Guid ID { get; set; }

        public List<SecretSantaModel> WhiteList { get; set; }
    }

}