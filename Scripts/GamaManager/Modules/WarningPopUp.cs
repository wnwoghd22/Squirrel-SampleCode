using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPopUp : MonoBehaviour
{
    [SerializeField] Text text;

    public void setText(string s)
    {
        text.text = s;
    }
}
