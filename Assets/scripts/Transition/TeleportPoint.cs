using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO sceneLoadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction(GameObject owner)
    {
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGo,true);
        Debug.Log("´«ËÍ");
    }
}
