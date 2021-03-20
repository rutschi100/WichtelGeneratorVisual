using System.Collections;
using System.Collections.Generic;

namespace WichtelGeneratorVisual
{
    public class WichtelModel
    {
        public string UserName { get; set; }
        public string GezogenerWichtel { get; set; }
        public List<string> BlackList { get; set; } = new List<string>();
        public List<string> WhiteList { get; set; } = new List<string>();
        public List<string> IamRegisteredBy { get; set; } = new List<string>();
        
        public WichtelModel(string userName)
        {
            UserName = userName;
            BlackList.Add(UserName);
        }
    }
}