using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine : MonoBehaviour
{
    public static UIStateMachine instance;
    [SerializeField] List<UIBaseStates> uiStatesList = new();
    private UIBaseStates currentState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        foreach (UIBaseStates state in uiStatesList)
        {
            state.gameObject.SetActive(false);
        }
        ChangeState(uiStatesList[0]);
    }
    public void ChangeState(UIBaseStates targetState)
    {
        currentState?.ExitState();
        currentState = targetState;
        currentState.EnterState();
    }
    public void ChangeState(uiStateName newState)
    {
        foreach(UIBaseStates state in uiStatesList)
        {
            if (state.GetUiStateName() == newState)
            {
                ChangeState(state);
                return;
            }
        }
    }
}
public enum uiStateName
{
    NetworkState,
    GameStartState,
    GamePlayState,
    GameOverState
}
public abstract class UIBaseStates : MonoBehaviour
{
    public uiStateName stateName;
    public abstract uiStateName GetUiStateName();
    public abstract void EnterState();
    public abstract void ExitState();
}
