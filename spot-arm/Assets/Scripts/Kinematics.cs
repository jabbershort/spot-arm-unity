using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace spot
{
    public class Kinematics
    {
        string folderPath = "";
    
        public static EEFPosition ForwardKinematics(Pose p)
        {
            string folderPath = Application.dataPath +"/StreamingAssets/ikpy/";
            string commandString = string.Format("python {0}/forward_kinematics.py {1} {2} {3} {4} {5} {6}",folderPath,p.jointAngles[0],p.jointAngles[1],p.jointAngles[2],p.jointAngles[3],p.jointAngles[4],p.jointAngles[5]);
            
            string output = Utility.RunCommandWithOutput(commandString);
            float[] f =Utility.GetFloatArrayFromString(output);
            return new EEFPosition(f);
        }
    
        public static Pose InverseKinematics(EEFPosition p)
        {
            string folderPath = Application.dataPath +"/StreamingAssets/ikpy/";
            string commandString = string.Format("python {0}/inverse_kinematics.py {1} {2} {3} {4} {5} {6}",folderPath,p.transform[0],p.transform[1],p.transform[2],p.transform[3],p.transform[4],p.transform[5]);
            
            string output = Utility.RunCommandWithOutput(commandString);
            Debug.Log(output);
            float[] f =Utility.GetFloatArrayFromString(output);
            return new Pose(f,0);
        }
    
    }
}


