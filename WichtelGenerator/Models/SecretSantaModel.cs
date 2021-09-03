using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WichtelGenerator.Core.Models
{
    public class SecretSantaModel
    {
        [Key]
        public Guid ID { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string MailAdress { get; set; }
        
        public Guid? ChoiseID { get; set; }
        public SecretSantaModel Choise { get; set; }

        //public IList<Guid> BlackListID {get;set;}
        //[ForeignKey("BlackListID")]
        public List<SecretSantaModel> BlackList { get; } = new List<SecretSantaModel>();

        //public IList<Guid> WhiteListID {get;set;}
        //[ForeignKey("WhiteListID")]
        public List<SecretSantaModel> WhiteList { get; } = new List<SecretSantaModel>();
    }
}