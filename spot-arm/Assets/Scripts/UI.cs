using UnityEngine;

namespace spot
{
    public class UI : MonoBehaviour
    {

        public GameObject arm;
        void Start()
        {

        }

        void Update()
        {

        }

        public void SendRetract()
        {
            arm.GetComponent<spot.ArmController>().driveToPose(spot.Enums.retracted); 
        }

        public void SendStraight()
        {
            arm.GetComponent<spot.ArmController>().driveToPose(spot.Enums.straight);
        }
        public void SendHome()
        {
            arm.GetComponent<spot.ArmController>().driveToPose(spot.Enums.home);
        }
    }
}