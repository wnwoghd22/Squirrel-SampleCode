using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IGameState
{
    /**
     * UniTask Start(ISceneHandler gm); -> 패러미터 변경 예정.
     * 씬 내부에서 호출해야 할 모든 요소들을 리팩터링할 필요 있음
     */
    UniTask Start(ISceneHandler gm);
    void Update(ISceneHandler gm);
    void HandleInput(ISceneHandler gm);
    void Enter(ISceneHandler gm);
    void Exit(ISceneHandler gm);

}
