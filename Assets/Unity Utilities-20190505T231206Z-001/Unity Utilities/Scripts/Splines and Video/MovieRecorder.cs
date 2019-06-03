using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieRecorder : MonoBehaviour {
    public int resWidth = 1920;
    public int resHeight = 1080;


    static int index = 0;
    public static string ScreenShotName(int width, int height)
    {
        //return string.Format("{0}/screenshots/movie_screen_{1}x{2}_{3}.jpg",
        //Application.dataPath,
        //width, height,
        //System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        index++;
        return "/Users/zannorman/Desktop/screenshots/img"+index.ToString("0000")+ ".jpg";
    }


    float t = 0;
    int fps = 60;
    void LateUpdate()
    {
        t -= Time.deltaTime;
        if (t < 0) {
            t = 1f / (float)fps;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            //Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.DXT1Crunched, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToJPG(35);
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            //Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }


    }
}
