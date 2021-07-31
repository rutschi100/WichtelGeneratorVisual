using System.Collections.Generic;

namespace WichtelGenerator.Core.Models
{
    public class SecretSantaModel
    {
        public string Name { get; set; }
        public string MailAdress { get; set; }
        public SecretSantaModel Choise { get; set; }
        public List<SecretSantaModel> BlackList { get; } = new List<SecretSantaModel>();
        public List<SecretSantaModel> WhiteList { get; } = new List<SecretSantaModel>();
    }
}