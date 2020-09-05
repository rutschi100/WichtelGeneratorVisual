using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string userName;
        private ArrayList blackList                    = new ArrayList();
        public List<string> WhiteList { get; set; } = new List<string>();
        private ArrayList binInBlackListEingetragenVon = new ArrayList();
        private string gezogenerWichtel;

        //============Getter Setter=========================================
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        //-------------------------
        public string GezogenerWichtel { get => gezogenerWichtel; set => gezogenerWichtel = value; }

        //-------------------------
        public void FillBlackList(string aUserToLock)
        {
            this.blackList.Add(aUserToLock);
        }

        //-------------------------
        public ArrayList GetBlackList()
        {
            return this.blackList;
        }

        //-------------------------
        public void SetBlackList(string aBlackListUser)
        {
            blackList.Add(aBlackListUser);
        }

        //-------------------------
        public void RemoveItemInBlackList(string aItem)
        {
            this.blackList.Remove(aItem);
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
            this.WhiteList.Add(aItem);
        }

        //-------------------------
        public void RemoveItemFromWhiteList(string aItem)
        {
            this.WhiteList.Remove(aItem);
        }

        public ArrayList GetbinInBlackListEingetragenVon()
        {
            return binInBlackListEingetragenVon;
        }

        public void SetbinInBlackListEingetragenVon(string aUser)
        {
            binInBlackListEingetragenVon.Add(aUser);
        }

        public void RemoveItemBinInBlackListEingetragenVon(string aItem)
        {
            binInBlackListEingetragenVon.Remove(aItem);
        }
    }
}
//======================================================
//===============END OF FILE============================
//======================================================
