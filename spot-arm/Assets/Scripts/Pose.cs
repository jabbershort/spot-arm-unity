using System;

namespace spot
{
    public class Pose
    {
        public float[] jointAngles;

        public Pose(float[] angles)
        {
            jointAngles = angles;
        }

        public override string ToString()
        {
            string s = "Pose: ";
            for (int i = 0; i < jointAngles.Length; i++)
            {
                s += Math.Round(jointAngles[i],2).ToString() +",";
            }
            return s;
        }

    }
}