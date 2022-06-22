using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

namespace spot
{
        public enum AngleUnit
        {
            Radiant,
            Degrees
        };

        public enum AxisSequence
        {
            ZYX,
            ZYZ,
            XYZ
        };

    public static class Utility
    {
        public static string RunCommandWithOutput(string command)
        {
            UnityEngine.Debug.Log("Running command: "+command);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.FileName = "CMD.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/C "+command;

            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public static float[] GetFloatArrayFromString(string response)
        {
            string[] outputLines = response.Split(new string[] {Environment.NewLine,"\n"},StringSplitOptions.RemoveEmptyEntries);
            string positionString = outputLines[outputLines.Length-1];

            string[] charsToRemove = new string[] {"[","]"};
            foreach (var c in charsToRemove)
            {
                positionString = positionString.Replace(c,string.Empty);
            }

            string[] angleString = positionString.Split(new string[] {","},StringSplitOptions.RemoveEmptyEntries);

            float[] f = new float[6];

            for(int i = 0;i<6;i++)
            {
                f[i] = float.Parse(angleString[i]);

            }

            return f;
        }

        public static float[] GetFloatArrayFromIKFastString(string response, Pose p)
        {
            string[] lines = response.Split(new string[] {Environment.NewLine,"\n"},StringSplitOptions.RemoveEmptyEntries);
            List<float[]> possibleSolutions = new List<float[]>();
            for (int i = 1; i<lines.Length;i++)
            {
                string line = lines[i];
                string[] segments = line.Split(new string[]{":",","},StringSplitOptions.RemoveEmptyEntries);
                List<string> angles = segments.ToList();
                angles.RemoveAt(0);
                angles.RemoveAt(2);
                float[] f = new float[6];
                for (int j = 0; j<6; j++)
                {
                    f[j] = float.Parse(angles[j]);
                }
                possibleSolutions.Add(convertToDegrees(f));
            }

            float[] closestSolution = new float[6];
            if (p is null)
            {
                p = Pose.home;
            }
            float minDistance = distanceToSolution(p.jointAngles,possibleSolutions[0]);
            float[] solution = possibleSolutions[0];
            foreach(float[] sol in possibleSolutions)
            {
                float distance = distanceToSolution(p.jointAngles,sol);
                UnityEngine.Debug.Log(distance);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    solution = sol;
                }
            }
            return solution;

        }

        private static float[] convertToRadians(float[] degrees)
        {
            float[] radians = new float[degrees.Length];
            for(int i = 0;i<degrees.Length;i++)
            {
                radians[i] = degrees[i]*Mathf.Deg2Rad;
            }
            return radians;
        }

        private static float[] convertToDegrees(float[] radians)
        {
            float[] degrees = new float[radians.Length];
            for(int i = 0;i<radians.Length;i++)
            {
                degrees[i] = radians[i]*Mathf.Rad2Deg;
            }
            return degrees;
        }

        public static float clampAngle(float angle)
        {
            float maximum, minimum, outputAngle, fullRevolution;
            maximum = 180.0f;
            minimum = -180.0f;
            fullRevolution = 360;

            if (angle > maximum)
            {
                // Debug.Log(string.Format("angle {0} is bigger than maximum",angle));
                outputAngle = -(fullRevolution - angle);
            }
            else if (angle < minimum)
            {
                // Debug.Log(string.Format("angle {0} is smaller than maximum",angle));
                outputAngle = fullRevolution + angle;
            }
            else
            {
                // Debug.Log(string.Format("angle {0} is fine",angle));
                outputAngle = angle;
            }
            // Debug.Log(string.Format("changing angle from {0} to {1}.",angle,outputAngle));
            return outputAngle;
        }

        private static float distanceToSolution(float[] p1, float[] p2)
        {
            float sum = 0;
            for(int i = 0;i<p1.Length;i++)
            {
                sum += Mathf.Abs(Mathf.Abs(clampAngle(p1[i]))-Mathf.Abs(clampAngle(p2[i])));
            }
            return sum;
        }

        public static float[] RotM2Eul(Matrix<float> R, AxisSequence sequence = AxisSequence.ZYZ, AngleUnit angleUnit = AngleUnit.Radiant)
        {
            //this can be done with a Matrix4x4.Rotation

            // Debug.Log(R);
            if (R.RowCount != 3 && R.ColumnCount != 3)
            {
                throw new ArgumentOutOfRangeException("The rotation matrix R must have 3x3 elements.");
            }

            float[] eul = new float[3];
            int firstAxis = 0;
            bool repetition = false;
            int parity = 0;
            int i = 0;
            int j = 0;
            int k = 0;
            int[] nextAxis = { 2, 3, 1, 2 };
            switch (sequence)
            {
                case AxisSequence.ZYX:
                    firstAxis = 3;
                    repetition = false;
                    parity = 0;
                    break;
                case AxisSequence.XYZ:
                    firstAxis = 1;
                    repetition = false;
                    parity = 0;
                    break;
                case AxisSequence.ZYZ:
                    firstAxis = 3;
                    repetition = true;
                    parity = 0;
                    break;
                default:
                    break;
            }
            i = firstAxis - 1;
            j = nextAxis[i + parity] - 1;
            k = nextAxis[i - parity + 1] - 1;

            if (repetition)
            {
                float sy = Mathf.Sqrt(R[i, j] * R[i, j] + R[i, k] * R[i, k]);
                bool singular = sy < 10 * Double.Epsilon;

                eul[0] = Mathf.Atan2(R[i, j], R[i, k]);
                eul[1] = Mathf.Atan2(sy, R[i, i]);
                eul[2] = Mathf.Atan2(R[j, i], -R[k, i]);

                if (singular)
                {
                    eul[0] = Mathf.Atan2(-R[j, k], R[j, j]);
                    eul[1] = Mathf.Atan2(sy, R[i, i]);
                    eul[2] = 0;
                }
            }
            else
            {
                float sy = Mathf.Sqrt(R[i, i] * R[i, i] + R[j, i] * R[j, i]);
                bool singular = sy < 10 * double.Epsilon;

                eul[0] = Mathf.Atan2(R[k, j], R[k, k]);
                eul[1] = Mathf.Atan2(-R[k, i], sy);
                eul[2] = Mathf.Atan2(R[j, i], R[i, i]);

                if (singular)
                {
                    eul[0] = Mathf.Atan2(-R[j, k], R[j, j]);
                    eul[1] = Mathf.Atan2(-R[k, i], sy);
                    eul[2] = 0;
                }
            }

            if (parity == 1)
            {
                eul[0] = -eul[0];
                eul[1] = -eul[1];
                eul[2] = -eul[2];
            }


            float value0 = eul[0];
            float value2 = eul[2];

            eul[0] = value2;
            eul[2] = value0;

            if (angleUnit == AngleUnit.Degrees)
            {
                eul[0] *= (180 / Mathf.PI);
                eul[1] *= (180 / Mathf.PI);
                eul[2] *= (180 / Mathf.PI);
            }
            if (sequence == AxisSequence.ZYX)
            {
                float[] output = new float[3] { eul[2], eul[1], eul[0] };
                return output;
            }
            return eul;
        }
    }
}