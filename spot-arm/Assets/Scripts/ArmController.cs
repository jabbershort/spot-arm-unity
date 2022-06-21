using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
namespace spot
{
    public class ArmController : MonoBehaviour
    {
        public ArticulationBody[] joints;

        private GameObject arm;

        private float[] jointAngles;
        public spot.Pose pose;

        public float[] angleMinVals;
        public float[] angleMaxVals;

        void Awake()
        {
            arm = gameObject;
            joints = arm.GetComponentsInChildren<ArticulationBody>();
            jointAngles = new float[7];
            angleMinVals = new float[7];
            angleMaxVals = new float[7];
            updateJointAngles();

            for (int i = 1;i<8;i++)
            {   
                angleMaxVals[i-1] = (joints[i].xDrive.upperLimit);
                angleMinVals[i-1] = (joints[i].xDrive.lowerLimit);
                Debug.Log(string.Format("Joint limits for {0} are {1} to {2}.",joints[i].name,joints[i].xDrive.upperLimit,joints[i].xDrive.lowerLimit));
            }
            Application.targetFrameRate = 30;
        }

        void Start()
        {
            jumpToPose(spot.Pose.retracted);
            spot.Pose.GetSavedPoses();
        }

        void savePoses()
        {
            List<spot.Pose> poses = new List<Pose>{spot.Pose.retracted,spot.Pose.home,spot.Pose.straight};
            string data = JsonConvert.SerializeObject(poses);
            File.WriteAllText(Application.dataPath+"/StreamingAssets/saved_poses.json",data);
        }

        // Update is called once per frame
        void Update()
        {
            updateJointAngles();
            // Debug.Log(joints.Length);
            // Debug.Log(pose.ToString());
            // Debug.Log(pose.endEffectorPosition.ToString());
        }

        void updateJointAngles()
        {
            for (int i = 1;i<8;i++)
            {
                // Debug.Log(string.Format("Joint: {0} at {1}.",joints[i].name,joints[i].jointPosition[0]));
                jointAngles[i-1] = Mathf.Rad2Deg * joints[i].jointPosition[0];
            }
            pose = new Pose(jointAngles,0);
        }

        public void jumpToPose(spot.Pose p)
        {
            Debug.Log("Jumping to pose: "+p.ToString());
            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                Debug.Log(string.Format("Driving joint {0} to {1}.",joints[i+1],p.jointAngles[i]));
                joints[i+1].jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*p.jointAngles[i]);
                ArticulationDrive drive = joints[i+1].xDrive;
                drive.target = p.jointAngles[i];
                joints[i+1].xDrive = drive;
            }
            joints[8].jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*p.gripper);
            ArticulationDrive gripperDrive = joints[8].xDrive;
            gripperDrive.target = p.gripper;
            joints[8].xDrive = gripperDrive;
        }



        public void driveToPose(spot.Pose p)
        {
            Debug.Log("Driving to pose.: "+p.ToString());
            Debug.Log(joints);
            Debug.Log(joints.Length);

            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                Debug.Log(string.Format("Driving joint {0} to {1}.",joints[i+1],p.jointAngles[i]));
                ArticulationDrive drive = joints[i+1].xDrive;
                drive.target = p.jointAngles[i];
                joints[i+1].xDrive = drive;
            }
            ArticulationDrive gripperDrive = joints[8].xDrive;
            gripperDrive.target = p.gripper;
            joints[8].xDrive = gripperDrive;
        }

        public void SetJointToPosition(int jointIndex, float val)
        {
            joints[jointIndex+1].GetComponent<ArticulationBody>().jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*val);
            ArticulationDrive drive = joints[jointIndex+1].xDrive;
            drive.target = Mathf.LerpAngle(jointAngles[jointIndex+1], val, 1.0f);
            joints[jointIndex+1].xDrive = drive;
        }
    }
}

