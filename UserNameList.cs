using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WichtelGeneratorVisual
{
    struct UserNameList
    {
       static ArrayList theList = new ArrayList();

        public ArrayList getTheList()
        {
            return theList;
        }

        public void setItemToTheList(string aItem)
        {
            theList.Add(aItem);
        }
    }
}
