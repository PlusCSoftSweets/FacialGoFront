using OpenCVForUnity;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class FaceDiscr : MonoBehaviour
{

    public GameObject uploaderPrefab;
    private int vac;
    public static FaceDiscr FaceDiscr_singleinstance = null;
    String front_face_haar = "haarcascade_frontalface_alt2.xml";
    MatOfRect faces;
    MatOfRect smile_faces;
    MatOfRect eye_glass;
    CascadeClassifier cascade;
    CascadeClassifier cascade_eye_mouth;
    CascadeClassifier cascade_smile;
    String face_cascade_name = "haarcascade_frontalface_alt2.xml";
    String eyes_cascade_name = "haarcascade_eye.xml";
    String smile_cascade_name = "haarcascade_smile.xml";

    private FaceDiscr()
    {
    }
    private void Awake()
    {
        if (FaceDiscr_singleinstance == null)
        {
            FaceDiscr_singleinstance = this;
            vac = 0;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    public static FaceDiscr GetInstance()
    {
        return FaceDiscr_singleinstance;
    }
    public double Face_Getpercent(int face_number, Mat faceMat, Mat grayMat, int frac)
    {
        
        Texture2D t1d = new Texture2D(grayMat.width(), grayMat.height());
        Utils.matToTexture2D(grayMat, t1d);

        var bytes2 = t1d.EncodeToPNG();
        Upload(bytes2,vac);
        vac++;
        List<Mat> images = new List<Mat>();
        List<int> labelsList = new List<int>();
       
        Debug.Log(frac);
        //MatOfInt labels = new MatOfInt();
        /*
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            
        });*/
        front_face_haar = Utils.getFilePath("haarcascade_frontalface_alt2.xml");
        smile_cascade_name = Utils.getFilePath(smile_cascade_name);
        eyes_cascade_name = Utils.getFilePath(eyes_cascade_name);
        Imgproc.cvtColor(faceMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
        Imgproc.equalizeHist(grayMat, grayMat);

        if (face_number == 0)
        {
            Debug.Log(1);
            cascade = new CascadeClassifier(front_face_haar);
            cascade_smile = new CascadeClassifier(smile_cascade_name);
            cascade_eye_mouth = new CascadeClassifier(eyes_cascade_name);
            faces = new MatOfRect();
            smile_faces = new MatOfRect();
            eye_glass = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                    new Size(200, 200), new Size());
            else return 0.5;
            OpenCVForUnity.Rect[] rects = faces.toArray();
            Debug.Log(rects.Length);
            if (rects.Length == 0) return 0.5;
            for (int i = 0; i < rects.Length; i++)
            {
                Mat Roifacemat = grayMat.submat(rects[i]);
                Texture2D t2d = new Texture2D(Roifacemat.width(), Roifacemat.height());
                Utils.matToTexture2D(Roifacemat, t2d);

                var bytes = t2d.EncodeToPNG();
                //Upload(bytes,-1*frac);
                
                Size dsize = new Size(200, 200);
                Mat testSampleMat = new Mat(dsize, Imgproc.COLOR_RGBA2GRAY); //Imgproc.COLOR_RGBA2GRAY
                Imgproc.resize(faceMat.submat(rects[i]), testSampleMat, dsize);
                testSampleMat = Stressed(testSampleMat);
                Texture2D t3d = new Texture2D(testSampleMat.width(), testSampleMat.height());
                Utils.matToTexture2D(testSampleMat, t3d);

                bytes = t3d.EncodeToPNG();
                Upload(bytes, vac);
                vac++;

                if (rects.Length > 0) return 0.8;

                if (cascade_smile != null)
                    cascade_smile.detectMultiScale(Roifacemat, smile_faces, 1.1, 55, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                        new Size(200, 200), new Size());
                OpenCVForUnity.Rect[] smile_rects = faces.toArray();
                cascade_eye_mouth.detectMultiScale(Roifacemat, eye_glass, 1.1, 2, 0 | 2, new Size(50, 50), new Size());
                OpenCVForUnity.Rect[] eyes = eye_glass.toArray();
                Debug.Log(13);
                if (rects.Length > 0) return 0.8;
                if (eyes.Length > 0 && rects.Length > 0) return 1;
                if (frac <= 2) return 0.6;
                if (eyes.Length >= 0 && rects.Length > 0) return 0.8;
            }
        }
        if (face_number == 1)
        {

            Debug.Log(1);
            cascade = new CascadeClassifier(front_face_haar);
            cascade_smile = new CascadeClassifier(smile_cascade_name);
            cascade_eye_mouth = new CascadeClassifier(eyes_cascade_name);
            faces = new MatOfRect();
            smile_faces = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                    new Size(200, 200), new Size());
            else return 0.5;
            OpenCVForUnity.Rect[] rects = faces.toArray();
            Debug.Log(rects.Length);
            if (rects.Length == 0) return 0.5;
            for (int i = 0; i < rects.Length; i++)
            {
                Mat Roifacemat = grayMat.submat(rects[i]);
                Texture2D t2d = new Texture2D(Roifacemat.width(), Roifacemat.height());
                Utils.matToTexture2D(Roifacemat, t2d);
                var bytes = t2d.EncodeToPNG();

                //Upload(bytes,-1*frac);

                Size dsize = new Size(200, 200);
                Mat testSampleMat = new Mat(dsize, Imgproc.COLOR_RGBA2GRAY); //Imgproc.COLOR_RGBA2GRAY
                Imgproc.resize(faceMat.submat(rects[i]), testSampleMat, dsize);
                testSampleMat = Stressed(testSampleMat);

                Texture2D t3d = new Texture2D(testSampleMat.width(), testSampleMat.height());
                Utils.matToTexture2D(testSampleMat, t3d);

                bytes = t3d.EncodeToPNG();
                Upload(bytes, vac);
                vac++;

                if (rects.Length > 0) return 0.8;

                if (cascade_smile != null)
                    cascade_smile.detectMultiScale(Roifacemat, smile_faces, 1.1, 55, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                        new Size(50, 50), new Size());
                OpenCVForUnity.Rect[] smile_rects = faces.toArray();
                cascade_eye_mouth.detectMultiScale(Roifacemat, eye_glass, 1.1, 2, 0 | 2, new Size(50, 50), new Size());
                OpenCVForUnity.Rect[] eyes = eye_glass.toArray();

                Debug.Log(13);
                if (rects.Length > 0) return 0.8;
                if (eyes.Length > 0 && rects.Length > 0) return 1;
                if (frac <= 2) return 0.6;
                if (eyes.Length >= 0 && rects.Length > 0) return 0.8;
            }
        }
        if (face_number == 2)
        {

            Debug.Log(1);
            cascade = new CascadeClassifier(front_face_haar);
            cascade_smile = new CascadeClassifier(smile_cascade_name);
            cascade_eye_mouth = new CascadeClassifier(eyes_cascade_name);
            faces = new MatOfRect();
            smile_faces = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                    new Size(200, 200), new Size());
            else return 0.5;
            OpenCVForUnity.Rect[] rects = faces.toArray();
            Debug.Log(rects.Length);

            if (rects.Length == 0) return 0.5;
            for (int i = 0; i < rects.Length; i++)
            {

                Mat Roifacemat = grayMat.submat(rects[i]);
                Texture2D t2d = new Texture2D(Roifacemat.width(), Roifacemat.height());
                Utils.matToTexture2D(Roifacemat, t2d);
                var bytes = t2d.EncodeToPNG();

                //Upload(bytes,frac);

                Size dsize = new Size(200, 200);
                Mat testSampleMat = new Mat(dsize, Imgproc.COLOR_RGBA2GRAY); //Imgproc.COLOR_RGBA2GRAY
                Imgproc.resize(faceMat.submat(rects[i]), testSampleMat, dsize);

                testSampleMat= Stressed(testSampleMat);
                Texture2D t3d = new Texture2D(testSampleMat.width(), testSampleMat.height());


                Utils.matToTexture2D(testSampleMat, t3d);

                
                bytes = t3d.EncodeToPNG();
                Upload(bytes, vac);
                vac++;

                if (cascade_smile != null)
                    cascade_smile.detectMultiScale(grayMat, smile_faces, 1.1, 2, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                        new Size(200, 200), new Size());
                OpenCVForUnity.Rect[] smile_rects = faces.toArray();

                Debug.Log(13);
                if (smile_rects.Length > 0) return 1;

                if (rects.Length > 0) return 0.8;
                if (frac <= 2) return 0.6;
            }
        }
        if (face_number == 3)
        {
            cascade = new CascadeClassifier(front_face_haar);
            cascade_smile = new CascadeClassifier(smile_cascade_name);
            cascade_eye_mouth = new CascadeClassifier(eyes_cascade_name);
            faces = new MatOfRect();
            smile_faces = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                    new Size(200, 200), new Size());
            else return 0.5;
            OpenCVForUnity.Rect[] rects = faces.toArray();

            Debug.Log(rects.Length);

            if (rects.Length == 0) return 0.5;
            for (int i = 0; i < rects.Length; i++)
            {
                Mat Roifacemat = grayMat.submat(rects[i]);

                Texture2D t2d = new Texture2D(Roifacemat.width(), Roifacemat.height());
                Utils.matToTexture2D(Roifacemat, t2d);

                var bytes = t2d.EncodeToPNG();

                //Upload(bytes,-1*frac);
                Size dsize = new Size(200,200);
                Mat testSampleMat = new Mat(dsize, Imgproc.COLOR_RGBA2GRAY); //Imgproc.COLOR_RGBA2GRAY
                Imgproc.resize(faceMat.submat(rects[i]), testSampleMat, dsize);

                testSampleMat = Stressed(testSampleMat);

                Texture2D t3d = new Texture2D(testSampleMat.width(), testSampleMat.height());
                Utils.matToTexture2D(testSampleMat, t3d);
                bytes = t3d.EncodeToPNG();

                Upload(bytes, vac);
                vac++;

                if (rects.Length > 0) return 0.8;
                if (cascade_smile != null)
                    cascade_smile.detectMultiScale(Roifacemat, smile_faces, 1.1, 55, 2, // TODO: objdetect.CV_HAAR_SCALE_IMAGE
                        new Size(200, 200), new Size());
                OpenCVForUnity.Rect[] smile_rects = faces.toArray();
                cascade_eye_mouth.detectMultiScale(Roifacemat, eye_glass, 1.1, 2, 0 | 2, new Size(200, 200), new Size());
                OpenCVForUnity.Rect[] eyes = eye_glass.toArray();

                Debug.Log(13);

                if (eyes.Length > 0) return 1;
                if (frac <= 2) return 0.6;
            }
        }
        return 0.0f;
    }

    Mat Stressed(Mat Roifacemat)
    {

        /*Debug.Log("fuck" + Roifacemat.channels());

        Debug.Log("myL" + Roifacemat.total());
        for (int j = 0; j < Roifacemat.width(); j++)
            for (int k = 0; k < Roifacemat.height(); k++)
            {
                double[] data = new double[1];
                data = Roifacemat.get(j, k);
                if (data[0] < 0.3) data[0] -= 0.225;
                if (data[0] > 0.3) data[0] += 0.225;
                if (data[0] > 1) data[0] = 1;
                if (data[0] < 0) data[0] = 0;
                Roifacemat.put(j, k, data);
            }
        Size ke = new Size(3, 3);
        Mat afRoifacemat = new Mat(Roifacemat.size(), CvType.CV_8UC1);
        Imgproc.blur(Roifacemat, afRoifacemat, ke);*/
        return Roifacemat;
    }
    void Upload(byte[] bytes,int num)
    {
        // spawn an uploader
        var uploader = Instantiate(uploaderPrefab);
        StartCoroutine(UploadCo(uploader, bytes,num));
    }

    IEnumerator UploadCo(GameObject uploader, byte[] bytes,int num)
    {
        yield return new WaitUntil(()=>uploader.activeSelf);
        uploader.GetComponent<PhotoUploader>().Upload(bytes,num);
    }
}

