using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Hardcoded. Dont expect much!
/// </summary>
public class GameplayManager : MonoBehaviour {
    [Header("Variables required")]
    [SerializeField, Tooltip("Time Limit" +
        " for the winning condition since it will be counting")]
    protected float m_TimeLimit = 120;
    [SerializeField, Tooltip("UI Image for player win display")]
    Image m_PlayerWinDisplay;
    [SerializeField, Tooltip("UI Image for player lose display")]
    Image m_PlayerLoseDisplay;
    [SerializeField, Tooltip("General GameOver Display")]
    Image m_GameOverDisplay;
    [SerializeField, Tooltip("Text for displaying the timer that player has survived")]
    TextMeshProUGUI m_TimeCounterTxt;

    [Header("Debugging")]
    [SerializeField, Tooltip("Time counter to count time")]
    protected float m_TimeCounter = 0;

    public float TimeLeft
    {
        get { return m_TimeLimit - m_TimeCounter; }
    }

    private void OnEnable()
    {
        SubscriptionSystem.Instance.SubscribeEvent< BaseObject.OBJECT_TYPE>("HouseDestroyed", DestroyedHouse);
        SubscriptionSystem.Instance.SubscribeEvent< BaseObject.OBJECT_TYPE>("UnitDestroyed", DestroyedPlayer);
    }

    private void OnDisable()
    {
        DeinitEvents();
    }

    private void Update()
    {
        m_TimeCounter += Time.deltaTime;
        if (TimeLeft <= 0)
        {
            PlayerWin();
        }
    }

    protected void DestroyedHouse(BaseObject.OBJECT_TYPE _typeOfObj)
    {
        List< BaseObject> currentListOfBuildings = ObjectManager.Instance.GetList(_typeOfObj);
        if (currentListOfBuildings.Count == 0)
        {
            PlayerLose();
        }
    }

    protected void DestroyedPlayer(BaseObject.OBJECT_TYPE _typeOfObj)
    {
        List<BaseObject> currentListOfUnits = ObjectManager.Instance.GetList(_typeOfObj);
        if (currentListOfUnits.Count == 0)
        {
            PlayerLose();
        }
    }

    protected void PlayerWin()
    {
        m_PlayerWinDisplay.gameObject.SetActive(true);
        GameOver();
    }

    protected void PlayerLose()
    {
        m_PlayerLoseDisplay.gameObject.SetActive(true);
        m_TimeCounterTxt.text = "Survived Duration: " + m_TimeCounter;
        GameOver();
    }

    protected void GameOver()
    {
        if (m_GameOverDisplay)
        {
            m_GameOverDisplay.gameObject.SetActive(true);
        }
        // unsubscribe the events since we dont need it anymore!
        DeinitEvents();
    }

    protected void DeinitEvents()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent< BaseObject.OBJECT_TYPE>("HouseDestroyed", DestroyedHouse);
        SubscriptionSystem.Instance.UnsubscribeEvent< BaseObject.OBJECT_TYPE>("UnitDestroyed", DestroyedPlayer);
    }
}
