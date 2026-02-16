using TMPro;
using UnityEngine;

public class GameOverState : UIBaseStates
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Color winColor,loseColor;
    public const string winStr = "YOU WON";
    public const string loseStr = "YOU LOSE";
    void Awake()
    {
        stateName = uiStateName.GameOverState;
    }
    public override void EnterState()
    {
        gameObject.SetActive(true);
        GameManager.instance.OnGameOverEvent += GameOverEvent;
    }

    public override void ExitState()
    {
        GameManager.instance.OnGameOverEvent -= GameOverEvent;
        gameObject.SetActive(false);
    }

    public override uiStateName GetUiStateName()
    {
        return uiStateName.GameOverState;
    }
    void GameOverEvent(bool isWinner)
    {
        
        if (isWinner)
        {
            resultText.text = winStr;
            resultText.color = winColor;
        }
        else
        {
            resultText.text = loseStr;
            resultText.color = loseColor;
        }
    }
}
