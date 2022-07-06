using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewardHandler
{
    void GetReward(int index, MailSlot slot);
}
