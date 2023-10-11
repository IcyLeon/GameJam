using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int Coins;
    //private float RosePriceModifier = 1.0f;
    //private float TulipPriceModifier = 1.0f;
    //private float DaisyPriceModifier = 1.0f;
    //private float CommonPriceModifier = 1.0f;
    //private float BabyBreathPriceModifier = 1.0f;
    //private float LilyOfTheValleyPriceModifier = 1.0f;
    //private float RosePriceModifier = 1.0f;
    //private float RosePriceModifier = 1.0f;

    /*
    Tulip Upgrade
Daisy Upgrade
Common Upgrade
Baby Breath Upgrade
Lily of the Valley Upgrade
Orchid Upgrade
Uncommon Upgrade
Lavender Upgrade
Moonflower Upgrade
Peony Upgrade
Rare Upgrade
Better Bouquet
Supreme Bouquet


     */

    public int GetCoins()
    {
        return Coins;
    }

    public void SetCoins(int amt)
    {
        Coins = amt;
    }

    public Inventory(int Coins = 0)
    {
        this.Coins = Coins;
    }
}
