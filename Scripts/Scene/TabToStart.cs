using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TabToStart : MonoBehaviour
{
    public void Tab() => SceneManager.LoadScene("Lobby");
}
