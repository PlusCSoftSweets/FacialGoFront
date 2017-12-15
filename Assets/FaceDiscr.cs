using UnityEngine;
using System.Collections;
using OpenCvSharp;
//using OpenCvSharp.MachineLearning;

public class FaceDiscr : MonoBehaviour
{
    private CvCapture capture;



    private int maxPointNum;
    //private CvMemStorage storage;
    //private CvSeq<CvPoint> maxSeq;
    // Use this for initialization
    void Start()
    {
        //storage = Cv.CreateMemStorage(0);
        //test = new PointTest();
        //storage = Cv.CreateMemStorage(0);
        capture = Cv.CreateCameraCapture(0);


    }

    public double Discriminent(string path,int number)
    {
        IplImage src;
        if (number == 0)
            using (src = new IplImage("F:/Unity/Face0.png", LoadMode.GrayScale))
        if (number == 1)
            using (src = new IplImage("F:/Unity/Face1.png", LoadMode.GrayScale))
        if (number == 2)
            using (src = new IplImage("F:/Unity/Face2.png", LoadMode.GrayScale))
                        
        if (number == 3)
            using (src = new IplImage("F:/Unity/Face2.png", LoadMode.GrayScale))
        using (IplImage dst = new IplImage(src.Size, BitDepth.U8, 1))
        {
            src.Canny(dst, 50, 200);
            using (CvWindow window_src = new CvWindow("src image", src))
            using (CvWindow window_dst = new CvWindow("dst image", dst))
            {
                CvWindow.WaitKey();
            }
        }   
        return 0.5;
    }

    // Update is called once per frame
    unsafe void Update()
    {

        IplImage frame = Cv.QueryFrame(capture);
        //Debug.Log(frame.NChannels);

        Cv.ShowImage("frame", frame);






    }




    void OnDestroy()
    {
        //Cv.ReleaseMemStorage(storage);
        //Cv.ReleaseMemStorage(storage);
        Cv.DestroyAllWindows();
        Cv.ReleaseCapture(capture);

    }
}
