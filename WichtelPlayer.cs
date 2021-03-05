using System.Collections;
using System.Collections.Generic;

namespace WichtelGeneratorVisual
{
    public class WichtelPlayer
    {
        //=============Konstruktoren=============
        public WichtelPlayer(string aUserName)
        {
            UserName = aUserName;
            FillBlackList(UserName);
        }


        //============Private Variablen==========
        private readonly ArrayList _blackList = new ArrayList();
        public List<string> WhiteList { get; set; } = new List<string>();
        private readonly ArrayList _binInBlackListEingetragenVon = new ArrayList();

        //============Getter Setter=========================================
        public string UserName { get; set; }

        //-------------------------
        public string GezogenerWichtel { get; set; }

        //-------------------------
        public void FillBlackList(string aUserToLock)
        {
            _blackList.Add(aUserToLock);
        }

        //-------------------------
        public ArrayList GetBlackList()
        {
            return _blackList;
        }

        //-------------------------
        public void SetBlackList(string aBlackListUser)
        {
            _blackList.Add(aBlackListUser);
        }

        //-------------------------
        public void RemoveItemInBlackList(string aItem)
        {
            _blackList.Remove(aItem);
        }

        //-------------------------
        public List<string> GetWhiteList()
        {
            return WhiteList;
        }

        /*
        //-------------------------
        public void SetWhiteList(List<string> aWhiteListUser)
        {
            whiteList = aWhiteListUser;
        }
        */
        //-------------------------
        public void SetWhiteListItem(string aItem)
        {
            WhiteList.Add(aItem);
        }

        //-------------------------
        public void RemoveItemFromWhiteList(string aItem)
        {
            WhiteList.Remove(aItem);
        }

        public ArrayList GetbinInBlackListEingetragenVon()
        {
            return _binInBlackListEingetragenVon;
        }

        public void SetbinInBlackListEingetragenVon(string aUser)
        {
            _binInBlackListEingetragenVon.Add(aUser);
        }

        public void RemoveItemBinInBlackListEingetragenVon(string aItem)
        {
            _binInBlackListEingetragenVon.Remove(aItem);
        }
    }
}
//======================================================
//===============END OF FILE============================
//======================================================