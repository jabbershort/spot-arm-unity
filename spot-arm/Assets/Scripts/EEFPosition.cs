using UnityEngine;
using System;

namespace spot
{
    public class EEFPosition
    {
        public float[] transform;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xRot;
        public float yRot;
        public float zRot;

        public EEFPosition(float[] t)
        {
            transform = t;
        }

        public override string ToString()
        {
            string s = "Position: ";
            for (int i = 0; i < transform.Length; i++)
            {
                s += Math.Round(transform[i],2).ToString() +",";
            }
            s+=".";
            return s;
        }
    }
}