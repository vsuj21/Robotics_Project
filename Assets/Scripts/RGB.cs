using UnityEngine;
using System.IO;

public class RGB : MonoBehaviour
{
    public Camera rgbCamera;
    public string rgbDataFileName = "rgb_data.txt"; // File to save RGB data
    public int targetFrameRate = 30; // Target frame rate
    public int downsampleFactor = 2; // Downsample factor

    private RenderTexture renderTexture;
    private Texture2D texture;

    void Start()
    {
        renderTexture = new RenderTexture(rgbCamera.pixelWidth, rgbCamera.pixelHeight, 24);
        rgbCamera.targetTexture = renderTexture;

        texture = new Texture2D(renderTexture.width / downsampleFactor, renderTexture.height / downsampleFactor, TextureFormat.RGB24, false);

        // Capture RGB data once
        CaptureRGBData();
    }

    void CaptureRGBData()
    {
        // Ensure the camera has rendered the scene before reading pixels
        if (!rgbCamera.targetTexture.IsCreated())
            return;

        // Render the camera's view into the RenderTexture
        rgbCamera.Render();

        // Read pixels from the RenderTexture with downsampling
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Downsample the texture
        Texture2D downsampledTexture = DownsampleTexture(texture);

        // Save RGB data (pixel colors) to file
        SaveRGBData(downsampledTexture);
    }

    Texture2D DownsampleTexture(Texture2D sourceTexture)
    {
        int width = sourceTexture.width / downsampleFactor;
        int height = sourceTexture.height / downsampleFactor;
        Color[] pixels = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x] = sourceTexture.GetPixel(x * downsampleFactor, y * downsampleFactor);
            }
        }

        Texture2D downsampledTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        downsampledTexture.SetPixels(pixels);
        downsampledTexture.Apply();

        return downsampledTexture;
    }

    void SaveRGBData(Texture2D texture)
    {
        // Open a file for writing RGB data
        StreamWriter writer = new StreamWriter(rgbDataFileName);

        // Save RGB data (pixel colors) to file
        Color[] pixels = texture.GetPixels();
        foreach (Color pixel in pixels)
        {
            writer.WriteLine(pixel.r + "," + pixel.g + "," + pixel.b);
        }

        // Close the file
        writer.Close();
    }

    void OnDestroy()
    {
        // Clean up resources
        if (texture != null)
            Destroy(texture);
    }
}
