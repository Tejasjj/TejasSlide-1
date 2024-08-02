using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    // private bool isPlayerAssigned = false;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        Invoke("FindAndAssignPlayer", 2f); // call the function after 2 seconds
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (!isPlayerAssigned)
    //    {
    //        FindAndAssignPlayer();
    //    }
    //}

    void FindAndAssignPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("CameraLock");
        if (player != null)
        {
            virtualCamera.Follow = player.transform;
            //virtualCamera.LookAt = player.transform;
            //isPlayerAssigned = true;
            //Debug.Log("Player found and camera assigned");
        }
        else
        {
            //Debug.LogWarning("Player not found");
        }
    }

    
}
