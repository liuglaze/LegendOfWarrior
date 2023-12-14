using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecicalEnemy : MonoBehaviour
{
    public GameObject theEnemy;
    public GameObject trophy;
    public void transPort()
    {
        Vector3 pos=theEnemy.transform.position;
        trophy.transform.position=new Vector3(pos.x,pos.y+1,pos.z);
        trophy.SetActive(true);
    }
}
