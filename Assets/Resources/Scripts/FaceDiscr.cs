using UnityEngine;
using System.Collections;
using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.Utilities;
using OpenCvSharp.CPlusPlus;
using System;
//using OpenCvSharp.MachineLearning;

public class FaceDiscr : MonoBehaviour
{
    //private CvCapture capture;
    public static FaceDiscr uniqueInstance;

    private int width = 240;
    private int height = 320;
    private static readonly object locker = new object();
    private FaceDiscr()
    {
    }

    public void Awake()
    {
		if (uniqueInstance == null) {
			uniqueInstance = this;
			DontDestroyOnLoad (uniqueInstance);
		} else {
			Debug.LogError ("Multiple instance is not allowed");
			Destroy (this);
		}
    }

    private int maxPointNum;
    //private CvMemStorage storage;
    //private CvSeq<CvPoint> maxSeq;
    // Use this for initialization
    void Start()
    {
        //storage = Cv.CreateMemStorage(0);
        //test = new PointTest();
        //storage = Cv.CreateMemStorage(0);
        //capture = Cv.CreateCameraCapture(0);


    }
    //估值函数
    private double evaluate(Mat predicted, Mat actual)
    {
        int t = 0;
        int f = 0;
        for (int i = 0; i < actual.Rows; i++)
        {
            float p = predicted.Rows;
            float a = 0;
            if ((p >= 0.0 && a >= 0.0) || (p <= 0.0 && a <= 0.0))
            {
                t++;
            }
            else
            {
                f++;
            }
        }
        return (t * 1.0) / (t + f);
    }
    // 学习函数
    int f(float x, float y, int equation)
    {
        switch (equation)
        {
            case 0:
                return y > System.Math.Sin(x * 10) ? -1 : 1;
                break;
            case 1:
                return y > System.Math.Cos(x * 10) ? -1 : 1;
                break;
            case 2:
                return y > 2 * x ? -1 : 1;
                break;
            case 3:
                return y > System.Math.Tan(x * 10) ? -1 : 1;
                break;
            default:
                return y >  System.Math.Cos(x * 10) ? -1 : 1;
        }
    }
    //SVM 计算 W
    void svm(Mat trainingData, OpenCvSharp.CvMat trainingClasses, int number,double degree)
    {
        CvTermCriteria cvTermCriteria = new CvTermCriteria(0.4);
        CvSVMParams param = new CvSVMParams(SVMType.CSvc,
            SVMKernelType.Rbf,
            number,
            2.0,
            degree,
            20,
            7,
            0,
            trainingClasses,
            cvTermCriteria
            );

        // SVM training
        for (int i = 0; i < trainingData.Rows; i++)
        {

            float x = i * 3 + 1;
            double y = i * 0.5 + 1;

        }

        //cout << "Accuracy_{SVM} = " << evaluate(predicted, testClasses) << endl;
        //plot_binary(testData, predicted, "Predictions SVM");

        // plot support vectors
        /*if (plotSupportVectors)
        {
            cv::Mat plot_sv(size, size, CV_8UC3);
            plot_sv.setTo(cv::Scalar(255.0, 255.0, 255.0));

            int svec_count = svm.get_support_vector_count();
            for (int vecNum = 0; vecNum < svec_count; vecNum++)
            {
                const float* vec = svm.get_support_vector(vecNum);
                cv::circle(plot_sv, Point(vec[0] * size, vec[1] * size), 3, CV_RGB(0, 0, 0));
            }
            cv::imshow("Support Vectors", plot_sv);
        }*/
    }
    // main calculate training data
    public double Calc(Mat trainingnumber,int number,double degree)
    {
        var srcImage = trainingnumber;
        Cv2.ImShow("Source", srcImage);
        Cv2.WaitKey(1); // do events

        var grayImage = new Mat();
        Cv2.EqualizeHist(grayImage, grayImage);
        var cascade = new CascadeClassifier(@"\Assets\Raw\face1&face0.xml");
        var nestedCascade = new CascadeClassifier(@"\Assets\Raw\face2&face3.xml");
        var faces = cascade.DetectMultiScale(
            image: grayImage,
            scaleFactor: 1.1,
            minNeighbors: 2,
            flags: HaarDetectionType.DoRoughSearch | HaarDetectionType.ScaleImage,
            minSize: new Size(30, 30)
            );
        

        var rnd = new UnityEngine.Random();
        var count = 1;
        foreach (var faceRect in faces)
        {
            var detectedFaceImage = new Mat(srcImage, faceRect);
            Cv2.ImShow(string.Format("Face {0}", count), detectedFaceImage);
            Cv2.WaitKey(1); // do events

            var color = Scalar.FromRgb((int)UnityEngine.Random.Range(0,255), (int)UnityEngine.Random.Range(0, 255), (int)UnityEngine.Random.Range(0, 255));
            Cv2.Rectangle(srcImage, faceRect, color, 3);


            var detectedFaceGrayImage = new Mat();
            var nestedObjects = nestedCascade.DetectMultiScale(
                image: detectedFaceGrayImage,
                scaleFactor: 1.1,
                minNeighbors: 2,
                flags: HaarDetectionType.DoRoughSearch | HaarDetectionType.ScaleImage,
                minSize: new Size(30, 30)
                );

            foreach (var nestedObject in nestedObjects)
            {
                var center = new Point
                {
                    X = (int)(System.Math.Round(nestedObject.X + nestedObject.Width * 0.5, MidpointRounding.ToEven) + faceRect.Left),
                    Y = (int)(System.Math.Round(nestedObject.Y + nestedObject.Height * 0.5, MidpointRounding.ToEven) + faceRect.Top)
                };
                var radius = System.Math.Round((nestedObject.Width + nestedObject.Height) * 0.25, MidpointRounding.ToEven);
                Cv2.Circle(srcImage, center, (int)radius, color, thickness: 3);
            }

            count++;
        }
        double ans = f(number, (float)count, (int)degree);
        if (ans < 0.05) return 0.1; if (ans > 0.5) return ans; else return UnityEngine.Random.Range((float)ans, (float)0.5);
  
    }
    public double Discriminent(byte[] image,int number)
    {
        IplImage src;
        IplImage iplImage = Cv.CreateImageHeader(Cv.Size(width,height), BitDepth.F64, 3);
        Mat M = new Mat(iplImage);
        return (Calc(M,number,0.05));
    }

    // Update is called once per frame
    unsafe void Update()
    {

        //IplImage frame = Cv.QueryFrame(capture);
        //Debug.Log(frame.NChannels);

        //Cv.ShowImage("frame", frame);






    }




    void OnDestroy()
    {
        //Cv.ReleaseMemStorage(storage);
        //Cv.ReleaseMemStorage(storage);
        Cv.DestroyAllWindows();
        //Cv.ReleaseCapture(capture);

    }
}
