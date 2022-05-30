using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : ISoundHandler
{
    public int SE_CLICK => 0;
    public int SE_LOCKED => 1;
    public int SE_PAPER => 2;
    public int SE_ROLL => 3;
    public int SE_CHUCKLE => 4;
    public int SE_ROPE => 5;
    public int SE_POP => 6;
    public int SE_JINGLE => 7;
    public int SE_FLAP => 8;
    public int SE_TREMPOLIN => 9;
    public int SE_JUMP => 10;
    public int SE_GETITEM => 11;

    public int SE_PROLOG_1 => 12;
    public int SE_PROLOG_2 => 13;

    public void PlayBGM(int index) => soundManager.PlayBGM(index);
    public void PlayMain() => soundManager.PlayMain();
    public void PlayProlog() => soundManager.PlayProlog();

    public void PlaySE(int index) => soundManager.PlaySE(index);
}