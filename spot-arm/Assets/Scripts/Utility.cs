using System.Diagnostics;
using System;
using System.Collections.Generic;
namespace spot
{
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
    }
}