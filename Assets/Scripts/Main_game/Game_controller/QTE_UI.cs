using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class QTE_UI : MonoBehaviour
{
    public static QTE_UI Instance;

    public GameObject killerPanel;
    public GameObject victimPanel;

    private QuickTimeEvent qte;

    void Awake()
    {
        Instance = this;
        killerPanel.SetActive(false);
        victimPanel.SetActive(false);
    }

    public void StartKillerQTE()
    {
        killerPanel.SetActive(true);
    }

    public void StartVictimQTE()
    {
        victimPanel.SetActive(true);
    }

    public void EndQTE()
    {
        killerPanel.SetActive(false);
        victimPanel.SetActive(false);
    }

    void Update()
    {
        if (killerPanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            QuickTimeEvent.localQTE.GetComponent<QuickTimeEvent>().CmdAddKillerPoint();
        }

        if (victimPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            QuickTimeEvent.localQTE.GetComponent<QuickTimeEvent>().CmdAddVictimPoint();
        }
    }
}
