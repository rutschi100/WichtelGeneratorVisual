using System.Collections.Generic;

namespace WichtelGeneratorVisual.Models
{
    public class WichtelModel
    {
        public WichtelModel(string userName)
        {
            UserName = userName;
            BlackList.Add(UserName);
        }

        public string UserName { get; set; }
        public string GezogenerWichtel { get; set; }
        public List<string> BlackList { get; set; } = new List<string>();
        public List<string> WhiteList { get; set; } = new List<string>();
        public List<string> IamRegisteredBy { get; set; } = new List<string>();
    }
}