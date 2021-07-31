using System;
using System.Collections.Generic;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.SantaManaager
{
    public interface ISantaManager
    {
        List<SecretSantaModel> SecretSantaModels { get; }
        public EventHandler<EventArgs> NewUserAddedEvent { get; set; }
        SantaBlackListWishResult AddSantaToBlackList(SecretSantaModel owner, SecretSantaModel modelToBeMoved);
        void AddSantaToWhiteList(SecretSantaModel owner, SecretSantaModel modelToBeMoved);
        AddUserResult AddNewSanta(SecretSantaModel model);
        void RemoveSanta(SecretSantaModel model);
    }
}