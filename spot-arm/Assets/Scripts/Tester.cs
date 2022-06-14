using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using spot;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EEFPosition p = spot.Kinematics.ForwardKinematics(spot.Pose.home);
        Debug.Log(p.ToString());

        spot.Pose pos = spot.Kinematics.InverseKinematics(p);
        Debug.Log(pos.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
