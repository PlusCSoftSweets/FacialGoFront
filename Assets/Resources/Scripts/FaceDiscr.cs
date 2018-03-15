using UnityEngine;

public class FaceDiscr
{
    public static FaceDiscr FaceDiscr_singleinstance = null;
    private FaceDiscr()
    {
    }
    public static FaceDiscr GetInstance()
    {
        if (FaceDiscr_singleinstance == null)
        {
            FaceDiscr_singleinstance = new FaceDiscr();
        }
        return FaceDiscr_singleinstance;
    }
    public double Face_Getpercent(int face_number, byte[] imageBytes)
    {
        return 0.8;
    }
}

