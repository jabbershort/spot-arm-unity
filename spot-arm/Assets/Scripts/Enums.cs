using spot;

namespace spot
{
    public class Enums
    {
        public static Pose retracted = new Pose(new float[7]{-180,0,180,0,0,0,20});
        public static Pose straight = new Pose(new float[7]{0,0,0,0,0,0,0});

        public static Pose home = new Pose(new float[7]{-180,45,150,0,15,0,-45});
    }

}