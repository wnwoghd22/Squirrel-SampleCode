using Cysharp.Threading.Tasks;
using System.Collections.Generic;

/**
 * 아직 메소드 추출중입니다...
 * 스토리 모드의 flow가 상당히 바뀔 예정이기 때문에
 * 단순한 리팩토링을 넘어 몇 가지는 추가되어야 할 듯 합니다.
 */

public interface ISceneHandler : ICanvasHandler, ISoundHandler, IDataHandler, IGameFlowHandler
{
    public Stack<IGameState> Stack { get; }
    public bool IsGameOver();

    public UniTask PushSceneState(IGameState state);
    public void PopSceneState();
    public void ExitGame();
}
