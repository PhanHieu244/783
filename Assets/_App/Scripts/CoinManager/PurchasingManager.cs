using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasingManager : MonoBehaviour
{
   public void OnPressDown(int i)
   {
      switch (i)
      {
         case 1:
            GameDataManager.Instance.playerData.AddDiamond(100);
             IAPManager.Instance.BuyProductID(IAPKey.PACK1_RE);
            break;
         case 2:
            GameDataManager.Instance.playerData.AddDiamond(300);
            IAPManager.Instance.BuyProductID(IAPKey.PACK2_RE);
            break;
         case 3:
            GameDataManager.Instance.playerData.AddDiamond(500);
            IAPManager.Instance.BuyProductID(IAPKey.PACK3_RE);
            break;
         case 4:
            GameDataManager.Instance.playerData.AddDiamond(1000);
            IAPManager.Instance.BuyProductID(IAPKey.PACK4_RE);
            break;
      }
   }

   public void Sub(int i)
   {
      GameDataManager.Instance.playerData.SubDiamond(i);
   }
}
