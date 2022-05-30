using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundHandler : ISEHandler
{
    void PlayBGM(int index);
    void PlayMain();
    void PlayProlog();
}
