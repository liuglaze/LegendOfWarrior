using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("广播")]
    public VoidEventSO SaveGameEventSO;
    public GameObject rock;
    public GameObject darkCharacter;
    public GameObject lightCharacter;
    // Start is called before the first frame update
    public void TriggerAction(GameObject owner)
    {
        darkCharacter.SetActive(false);
        lightCharacter.SetActive(true);
        //todo：保存数据
        SaveGameEventSO.RaiseEvent();
        rock.tag = "Untagged";
    }
}
