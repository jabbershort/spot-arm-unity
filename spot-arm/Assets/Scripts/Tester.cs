using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using spot;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EEFPosition p = spot.Kinematics.ForwardKinematicsPY(spot.Pose.home);
        Debug.Log(p.ToString());

        // spot.Pose pos = spot.Kinematics.InverseKinematics(p);
        // Debug.Log(pos.ToString());

        spot.Pose pos1 = spot.Kinematics.IKFast(p);
        Debug.Log(pos1.ToString());
        spot.Kinematics.IKFastDLL();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
