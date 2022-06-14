using System;
using System.Collections.Generic;

namespace spot
{
    public class Pose
    {
        public float[] jointAngles;
        public float gripper;
        public string name;

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

        public void SavePose()
        {

        }

        public static List<Pose> GetSavedPoses()
        {
            List<Pose> poses = new List<Pose>();
            
            // fetch poses from file

            return poses;
        }

        public static Pose retracted = new Pose(new float[6]{-180,0,180,0,0,0},20);
        public static Pose straight = new Pose(new float[6]{0,0,0,0,0,0},0);

        public static Pose home = new Pose(new float[6]{-180,-45,150,0,15,0},-45);

    }
}