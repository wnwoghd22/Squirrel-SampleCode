using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private GameObject eye;

    public void Twinkle()
    {
        UniTask.Create(async () => await TwinkleAsync());
    }
    private async UniTask TwinkleAsync()
    {
        eye.SetActive(true);

        await UniTask.Delay(1000);

        eye.SetActive(false);
    }
}
