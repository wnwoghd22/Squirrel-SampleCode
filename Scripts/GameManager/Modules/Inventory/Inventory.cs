using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IShopItemHandler
{
    public const int MAGNET = 0;
    public const int BUBBLE_SHIELD = 1;
    public const int BALLOON = 2;
    public const int INITSTAGE = 3;

    //private const int NUM_OF_ITEMS = 5;
    //private int[] items = new int[NUM_OF_ITEMS];

    private IGameFlowHandler flowHandler;
    private IFileHandler fileHandler;

    public void AddItem(int index, int num)
    {
        fileHandler.Data.MyItemCount[index] += num;
        //fileHandler.Save();

        //items[index] += num;
        //count[index].text = items[index].ToString();
    }

    /// <summary>
    /// 아이템을 사용 혹은 해제할 때 호출하는 메소드
    /// </summary>
    /// <param name="index"></param>
    /// <returns>아이템 사용에 성공했다면 true를 반환</returns>
    public bool SelectItem(int index)
    {
        bool result = flowHandler.ShopItemContainer[index] ? UnuseItem(index) : UseItem(index);
        //fileHandler.Save();

        return result;
    }
    public bool UseItem(int index)
    {
        if (fileHandler.Data.MyItemCount[index] < 1)
        {
            Debug.Log("No item! : " + index);
            return false;
        }

        Debug.Log("Use item : " + index);
        fileHandler.Data.MyItemCount[index]--;
        flowHandler.ShopItemContainer[index] = true;

        return true;
    }
    public bool UnuseItem(int index)
    {
        Debug.Log("Unuse item : " + index);
        fileHandler.Data.MyItemCount[index]++;
        flowHandler.ShopItemContainer[index] = false;

        return false;
    }

    void Start()
    {
        flowHandler = FindObjectOfType<GameManager>();
        fileHandler = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
    }
}
