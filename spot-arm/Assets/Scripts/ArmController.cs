using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
namespace spot
{
    public class ArmController : MonoBehaviour
    {
        private ArticulationBody[] joints;
        private ArticulationBody[] fullArticulationChain;
        private ArticulationBody gripper;

        private GameObject arm;

        private float[] jointAngles;
        public spot.Pose pose;

        public float[] angleMinVals;
        public float[] angleMaxVals;

        void Awake()
        {
            arm = gameObject;
            fullArticulationChain = arm.GetComponentsInChildren<ArticulationBody>();
            joints = new ArticulationBody[6];
            joints[0]=fullArticulationChain[1];
            joints[1]=fullArticulationChain[2];
            joints[2]=fullArticulationChain[4];
            joints[3]=fullArticulationChain[5];
            joints[4]=fullArticulationChain[6];
            joints[5]=fullArticulationChain[7];
            gripper = fullArticulationChain[8];
            jointAngles = new float[6];
            angleMinVals = new float[6];
            angleMaxVals = new float[6];
            updateJointAngles();

            for(int i = 0;i<joints.Length;i++)
            {   
                angleMaxVals[i] = (joints[i].xDrive.upperLimit);
                angleMinVals[i] = (joints[i].xDrive.lowerLimit);
                // Debug.Log(string.Format("Joint limits for {0} are {1} to {2}.",joints[i].name,joints[i].xDrive.upperLimit,joints[i].xDrive.lowerLimit));
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
            for (int i = 0;i<joints.Length;i++)
            {
                // Debug.Log(string.Format("Joint: {0} at {1}.",joints[i].name,joints[i].jointPosition[0]));
                jointAngles[i] = Mathf.Rad2Deg * joints[i].jointPosition[0];
            }
            pose = new Pose(jointAngles,0);
        }

        public void jumpToPose(spot.Pose p)
        {
            Debug.Log("Jumping to pose: "+p.ToString());
            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                // Debug.Log(string.Format("Driving joint {0} to {1}.",joints[i+1],p.jointAngles[i]));
                joints[i].jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*p.jointAngles[i]);
                ArticulationDrive drive = joints[i].xDrive;
                drive.target = p.jointAngles[i];
                joints[i].xDrive = drive;
            }
            gripper.jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*p.gripper);
            ArticulationDrive gripperDrive =gripper.xDrive;
            gripperDrive.target = p.gripper;
            gripper.xDrive = gripperDrive;
        }



        public void driveToPose(spot.Pose p)
        {
            Debug.Log("Driving to pose.: "+p.ToString());
            Debug.Log(joints);
            Debug.Log(joints.Length);

            for (int i = 0;i<p.jointAngles.Length;i++)
            {
                Debug.Log(string.Format("Driving joint {0} to {1}.",joints[i],p.jointAngles[i]));
                ArticulationDrive drive = joints[i].xDrive;
                drive.target = p.jointAngles[i];
                joints[i].xDrive = drive;
            }

            ArticulationDrive gripperDrive = gripper.xDrive;
            gripperDrive.target = p.gripper;
            gripper.xDrive = gripperDrive;
        }

        public void SetJointToPosition(int jointIndex, float val)
        {
            joints[jointIndex].GetComponent<ArticulationBody>().jointPosition = new ArticulationReducedSpace(Mathf.Deg2Rad*val);
            ArticulationDrive drive = joints[jointIndex].xDrive;
            drive.target = Mathf.LerpAngle(jointAngles[jointIndex], val, 1.0f);
            joints[jointIndex].xDrive = drive;
        }
    }
}

