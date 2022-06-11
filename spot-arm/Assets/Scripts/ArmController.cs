using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace spot
{
    public class ArmController : MonoBehaviour
    {

        private ArticulationBody[] joints;

        private GameObject arm;

        private float[] jointAngles;
        public spot.Pose pose;

        void Start()
        {
            arm = gameObject;
            joints = arm.GetComponentsInChildren<ArticulationBody>();
            jointAngles = new float[7];
            for(int i =0;i<joints.Length;i++)
            {
                Debug.Log(joints[i].gameObject.name);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // updateJointAngles();
            // Debug.Log(pose.ToString());
        }

        void updateJointAngles()
        {
            for (int i = 1;i<joints.Length;i++)
            {
                jointAngles[i] = Mathf.Rad2Deg * joints[i].jointPosition[0];
            }
            pose = new Pose(jointAngles);

        }

        public void jumpToPose(spot.Pose p)
        {
            Debug.Log("Jumping to pose: "+p.ToString());
            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                joints[i+1].jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*p.jointAngles[i]);
                ArticulationDrive drive = joints[i+1].xDrive;
                drive.target = p.jointAngles[i];
                joints[i+1].xDrive = drive;
            }
        }

        public void driveToPose(spot.Pose p)
        {
            Debug.Log("Driving to pose.: "+p.ToString());
            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                ArticulationDrive drive = joints[i+1].xDrive;
                drive.target = p.jointAngles[i];
                joints[i+1].xDrive = drive;
            }
        }
    }
}

