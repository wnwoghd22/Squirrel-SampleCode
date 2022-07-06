using UnityEngine;
using System.Collections.Generic;

public interface ICanvasHandler
{
    GameObject MainPageUI { get; }

    SelectChapterUI SelectChapterUI { get; }
    SelectStageUI SelectStageUI { get; }
    SelectItemUI SelectItemUI { get; }
    StageUI StageUI { get; }

    GameObject LobbyUI { get; }
    GameObject HideoutUI { get; }

    LobbyMenu LobbyMenu { get; }
    LobbyUpperBar LobbyUpperBar { get; }

    GameObject LoadingScene { get; }

    GameObject TutorialGuide { get; }

    GameObject ShowEndingBook { get; }
    void SetUpperBar();
}
