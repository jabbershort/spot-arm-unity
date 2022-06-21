using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System; 
using System.Threading;

namespace spot
{
    public class JointController : MonoBehaviour
    {
        [SerializeField]
        public List<Slider> JointControllers;
        private int selectedJoint =-1;

        public ArmController armController;   

        void Start()
        {
            // Set values of JointControllers depending on ArticulationBody
            foreach(Slider joint in JointControllers)
            {
                int jointIndex = JointControllers.IndexOf(joint);
                float minVal = armController.angleMinVals[jointIndex];
                float maxVal = armController.angleMaxVals[jointIndex];
                joint.minValue = minVal;
                joint.maxValue = maxVal;
            }
            
        }

        void Update()
        {
            // Get values of joint positions from DT and set values
            foreach(Slider joint in JointControllers)
            {
                int jointIndex = JointControllers.IndexOf(joint);
                // Debug.Log(jointIndex);
                float val = armController.pose.jointAngles[jointIndex];
                joint.gameObject.transform.parent.gameObject.GetComponentInChildren<Text>().text = string.Format("{0}:{1}",jointIndex+1,Math.Round((float)val,1));
                if (jointIndex == selectedJoint)
                {
                    float setVal = joint.value;
                    Debug.Log(string.Format("Setting joint {0} to {1}.",jointIndex,setVal));
                    armController.SetJointToPosition(jointIndex,setVal);
                }
                else
                {
                    joint.value = val;
                }
            }
        }

        public void SliderSelect(Slider sb)
        {
            int joint = JointControllers.IndexOf(sb);
            selectedJoint = joint;
        }

        public void SliderDeselect(Slider sb)
        {
            selectedJoint = -1;
        }


    }
}