using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WichtelGenerator.Core.Models
{
    public class SecretSantaModel
    {
        [Key] public Guid ID { get; set; }

        [Required] public string Name { get; set; }

        public string MailAdress { get; set; }

        public SecretSantaModel Choise { get; set; }

        public BlackListModel BlackListModel { get; set; } = new BlackListModel();

        public WhiteListModel WhiteListModel { get; set; } = new WhiteListModel();
    }

    public class BlackListModel
    {
        [Key] public Guid ID { get; set; }

        public List<SecretSantaModel> BlackList { get; set; } = new List<SecretSantaModel>();
    }

    public class WhiteListModel
    {
        [Key] public Guid ID { get; set; }

        public List<SecretSantaModel> WhitList { get; set; } = new List<SecretSantaModel>();
    }
}