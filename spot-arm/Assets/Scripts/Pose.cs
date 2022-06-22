using System;
using System.Collections.Generic;
using Newtonsoft;
using System.IO;
namespace spot
{
    public class Pose
    {
        public float[] jointAngles;
        public float gripper;
        public string name;
        // public EEFPosition endEffectorPosition {get {return _generateEEF();} private set{}}

        public Pose(float[] angles,float grip, string name = "")
        {
            jointAngles = angles;
            gripper = grip;
            this.name = name;
        }

        public override string ToString()
        {
            string s = string.Format("Pose {0}: ",name);
            for (int i = 0; i < jointAngles.Length; i++)
            {
                s += Math.Round(jointAngles[i],2).ToString() +",";
            }
            s += ". Gripper: ";
            s+=Math.Round(gripper,2);
            s+=".";
            return s;
        }

        // private EEFPosition _generateEEF()
        // {
        //     EEFPosition pos = Kinematics.ForwardKinematics(this);
        //     return pos;
        // }

        public void SavePose()
        {
            List<Pose> poses = GetSavedPoses();

            poses.Add(this);
            SavePoses(poses);
        }

        public double[] JointsAsDouble()
        {
            return Array.ConvertAll(jointAngles, x => (double)x);
        }

        private static void SavePoses(List<Pose> poses)
        {
            File.WriteAllText(UnityEngine.Application.dataPath+"/StreamingAssets/saved_poses.json",Newtonsoft.Json.JsonConvert.SerializeObject(poses));
        }

        public static Pose GetPose(string n)
        {
            List<Pose> poses = GetSavedPoses();

            foreach(Pose p in poses)
            {
                if (p.name == n)
                {
                    return p;
                }
            }
            return null;
        }

        public static List<Pose> GetSavedPoses()
        {
            List<Pose> poses = new List<Pose>();
            
            poses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pose>>(File.ReadAllText(UnityEngine.Application.dataPath+"/StreamingAssets/saved_poses.json"));
            // foreach(Pose p in poses)
            // {
            //     UnityEngine.Debug.Log("Loading pose: "+p.name);
            // }
            return poses;
        }

        public static Pose retracted = new Pose(new float[6]{0,-180,180,0,0,0},0,"retracted");
        public static Pose straight = new Pose(new float[6]{0,0,0,0,0,0},0,"straight");

        public static Pose home = new Pose(new float[6]{0,-45,90,0,-45,0},-45,"home");

    }
}