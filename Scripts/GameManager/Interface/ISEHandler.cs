using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISEHandler
{
    int SE_CLICK { get; }
    int SE_LOCKED { get; }
    int SE_PAPER { get; }
    int SE_ROLL { get; }
    int SE_CHUCKLE { get; }
    int SE_ROPE { get; }
    int SE_POP { get; }
    int SE_JINGLE { get; }
    int SE_FLAP { get; }
    int SE_TREMPOLIN { get; }
    int SE_JUMP { get; }
    int SE_GETITEM { get; }

    int SE_PROLOG_1 { get; }
    int SE_PROLOG_2 { get; }

    void PlaySE(int index);
}
