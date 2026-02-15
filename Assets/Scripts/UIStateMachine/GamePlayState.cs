using UnityEngine;
using UnityEngine.UI;

public class GamePlayState : UIBaseStates
{
    void Awake()
    {
        stateName = uiStateName.GamePlayState;
    }
    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }

    public override uiStateName GetUiStateName()
    {
        return uiStateName.GamePlayState;
    }
}
