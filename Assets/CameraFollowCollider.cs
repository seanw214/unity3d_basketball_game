using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowCollider : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCameraMain;
    public CinemachineVirtualCamera cinemachineCameraStationary;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cinemachineCameraMain.Priority = 9;
            cinemachineCameraStationary.Priority = 10;

            //cinemachineCameraStationary.Priority = 9;
            //cinemachineCameraMain.Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cinemachineCameraStationary.Priority = 9;
            cinemachineCameraMain.Priority = 10;

            //cinemachineCameraMain.Priority = 9;
            //cinemachineCameraStationary.Priority = 10;
        }
    }
}
