using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopItemHandler
{
    void AddItem(int index, int num);
    bool UseItem(int index);
}
