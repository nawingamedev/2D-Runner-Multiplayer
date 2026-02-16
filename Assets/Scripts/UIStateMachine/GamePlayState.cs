using UnityEngine;
using UnityEngine.UI;

public class GamePlayState : UIBaseStates
{
    [SerializeField] MovementUI movementUI;
    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        movementUI.isHolding = false;
        gameObject.SetActive(false);
    }

    public override uiStateName GetUiStateName()
    {
        return uiStateName.GamePlayState;
    }
}
