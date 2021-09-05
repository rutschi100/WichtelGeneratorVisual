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

        public SantaBlackList BlackList { get; set; }

        public SantaWhiteList WhiteList { get; set; }
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