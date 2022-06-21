using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace spot
{
    public class UI : MonoBehaviour
    {
        public Dropdown dropdown;
        
        public GameObject arm;
        void Start()
        {
            InitiliseDropDown();
        }

        void Update()
        {

        }


        private void InitiliseDropDown()
        {
            List<Pose> poses = spot.Pose.GetSavedPoses();
            dropdown.ClearOptions();
            List<Dropdown.OptionData> opts = new List<Dropdown.OptionData>();
            foreach(Pose p in poses)
            {
                opts.Add(new Dropdown.OptionData(p.name));
            }
            dropdown.AddOptions(opts);
        }

        public void Jump()
        {
            string selectedPose = dropdown.options[dropdown.value].text;
            arm.GetComponent<ArmController>().jumpToPose(spot.Pose.GetPose(selectedPose));
        }

        public void Drive()
        {
            string selectedPose = dropdown.options[dropdown.value].text;
            arm.GetComponent<ArmController>().driveToPose(spot.Pose.GetPose(selectedPose));
        }

        public void SendRetract()
        {
            arm.GetComponent<ArmController>().driveToPose(spot.Pose.retracted); 
        }

        public void SendStraight()
        {
            arm.GetComponent<ArmController>().driveToPose(spot.Pose.straight);
        }
        public void SendHome()
        {
            arm.GetComponent<ArmController>().driveToPose(spot.Pose.home);
        }
    }
}