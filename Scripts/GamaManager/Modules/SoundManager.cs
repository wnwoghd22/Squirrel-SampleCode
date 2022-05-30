using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider backgroundSlider;
    [SerializeField] Slider seSlider;

    [SerializeField] AudioSource background;
    public AudioSource Background => background;
    [SerializeField] AudioClip main;
    [SerializeField] AudioClip prolog;
    [SerializeField] AudioClip[] bgm;

    [SerializeField] AudioSource[] se;

    IDataHandler dataHandler;

    public void SetBackgroundVolume(float value)
    {
        background.volume = value;
        if (dataHandler?.Data != null)
            dataHandler.Data.BackgroundVolume = value;
        backgroundSlider.value = value;
    }

    public void SetSoundEffectVolume(float value)
    {
        foreach (AudioSource effect in se)
            effect.volume = value;
        if (dataHandler?.Data != null)
            dataHandler.Data.SoundEffectVolume = value;
        seSlider.value = value;
    }

    public void setting()
    {
        SetBackgroundVolume(dataHandler.Data.BackgroundVolume);
        SetSoundEffectVolume(dataHandler.Data.SoundEffectVolume);
    }

    async void Start()
    {
        background.loop = true; // 배경음은 반복 재생

        dataHandler = FindObjectOfType<GameManager>();

        await UniTask.WaitUntil(() => dataHandler?.Data != null);

        setting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGM(int index)
    {
        if (background.clip == bgm[index] && background.isPlaying) return;

        background.Stop();
        background.clip = bgm[index];
        background.Play();
    }

    public void PlayMain()
    {
        if (background.clip == main && background.isPlaying) return;

        background.Stop();
        background.clip = main;
        background.Play();
    }
    public void PlayProlog()
    {
        if (background.clip == prolog && background.isPlaying) return;

        background.Stop();
        background.clip = prolog;
        background.Play();
    }

    public void PlaySE(int index) => se[index].Play();


    public void PlaySEClick() => se[0].Play();
}
