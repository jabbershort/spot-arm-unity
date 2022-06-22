using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using MathNet.Numerics.LinearAlgebra;

namespace spot
{
    public class Kinematics
    {
        [DllImport("ikfast_robot.dll")]
        private static extern bool ComputeFk(double[] joints, ref double[] eetrans, ref double[] eerot);


        [DllImport("ikfast_robot.dll")]
        private static extern int GetNumFreeParameters();

        [DllImport("ikfast_robot.dll")]
        private static extern int[] GetFreeParameters();


        [DllImport("ikfast_robot.dll")]
        private static extern int GetNumJoints();

        [DllImport("ikfast_robot.dll")]
        private static extern int GetIkRealSize();


        [DllImport("ikfast_robot.dll")]
        private static extern char[] GetIkFastVersion();

        [DllImport("ikfast_robot.dll")]
        private static extern int GetIkType();
        
        string folderPath = "";
    
        public static EEFPosition ForwardKinematicsPY(Pose p)
        {
            string folderPath = Application.dataPath +"/StreamingAssets/ikpy/";
            string commandString = string.Format("python {0}/forward_kinematics.py {1} {2} {3} {4} {5} {6}",folderPath,p.jointAngles[0],p.jointAngles[1],p.jointAngles[2],p.jointAngles[3],p.jointAngles[4],p.jointAngles[5]);
            
            string output = Utility.RunCommandWithOutput(commandString);
            float[] f =Utility.GetFloatArrayFromString(output);
            return new EEFPosition(f);
        }
    
        public static Pose InverseKinematicsPY(EEFPosition p)
        {
            string folderPath = Application.dataPath +"/StreamingAssets/ikpy/";
            string commandString = string.Format("python {0}/inverse_kinematics.py {1} {2} {3} {4} {5} {6}",folderPath,p.transform[0],p.transform[1],p.transform[2],p.transform[3],p.transform[4],p.transform[5]);
            
            string output = Utility.RunCommandWithOutput(commandString);
            Debug.Log(output);
            float[] f =Utility.GetFloatArrayFromString(output);
            return new Pose(f,0);
        }

        public static Pose IKFast(EEFPosition p, Pose start = null)
        {
            string folderPath = Application.dataPath +"/StreamingAssets/";
            System.Numerics.Matrix4x4 rotMat = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(p.transform[4],p.transform[3],p.transform[5]);
            string commandString = string.Format("{13}/ikfast_robot.exe {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                rotMat.M11,rotMat.M12,rotMat.M13,p.transform[0],
                rotMat.M21,rotMat.M22,rotMat.M23,p.transform[1],
                rotMat.M31,rotMat.M32,rotMat.M33,p.transform[2],
                0,folderPath
                );
            
            string output = Utility.RunCommandWithOutput(commandString);
            float[] f = Utility.GetFloatArrayFromIKFastString(output,start);
            if (start == null)
            {
                return new Pose(f,0);
            }
            return new Pose(f,start.gripper);
        }

        public static EEFPosition ForwardKinematics(Pose p)
        {
            double[] eetrans = new double[3];
            double[] eerot = new double[9];
            bool result = ComputeFk(p.JointsAsDouble(),ref eetrans,ref eerot);
            float[] rot1 = new float[3]{(float)eerot[0],(float)eerot[1],(float)eerot[2]};
            float[] rot2 = new float[3]{(float)eerot[3],(float)eerot[4],(float)eerot[5]};
            float[] rot3 = new float[3]{(float)eerot[6],(float)eerot[7],(float)eerot[8]};
            Matrix<float> rotMat = Matrix<float>.Build.DenseOfRowArrays(rot1,rot2,rot3);
            float[] euler = Utility.RotM2Eul(rotMat,AxisSequence.XYZ,AngleUnit.Degrees);
            return new EEFPosition(new float[6]{(float)eetrans[0],(float)eetrans[1],(float)eetrans[2],euler[0],euler[1],euler[2]});
        }

        public static Pose InverseKinematics(EEFPosition p, Pose start = null)
        {
            return start;
        }

        public static void IKFastDLL()
        {
            Debug.Log(GetNumFreeParameters());
            // int[] result = GetFreeParameters();
            // Debug.Log(result);
            Debug.Log(GetNumJoints());
            Debug.Log(GetIkRealSize());
            // Debug.Log(GetIkFastVersion());
            Debug.Log(GetIkType());
            double[] joints = new double[7]{0,0,0,0,0,0,0};
            double[] eetrans= new double[3];
            double[] eerot= new double[9];
            bool result = ComputeFk(joints,ref eetrans,ref eerot);
            Debug.Log(result);
            Debug.Log(string.Format("EEtrans: {0}, {1}, {2}.",eetrans[0],eetrans[1],eetrans[2]));
            Debug.Log(string.Format("EErot: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                eerot[0],eerot[1],eerot[2],
                eerot[3],eerot[4],eerot[5],
                eerot[6],eerot[7],eerot[8]
                ));
        }

    
    }
}


