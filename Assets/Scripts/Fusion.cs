using UnityEngine;
using System.IO;

public class Fusion : MonoBehaviour
{
    public string lidarDataFileName = "lidar_data.txt";
    public string rgbDataFileName = "rgb_data.txt";
    public float obstacleThreshold = 5f; // Distance threshold for detecting obstacles

    private struct LiDARPoint
    {
        public float distance;
        public string obstacleName;
    }

    private struct RGBPixel
    {
        public float r;
        public float g;
        public float b;
    }

    private LiDARPoint[] lidarData;
    private RGBPixel[] rgbData;

    void Start()
    {
        // Read LiDAR data
        ReadLiDARData();

        // Read RGB data
        ReadRGBData();

        // Perform fusion
        PerformFusion();
    }

    void ReadLiDARData()
    {
        // Read LiDAR data from file
        string[] lines = File.ReadAllLines(lidarDataFileName);
        lidarData = new LiDARPoint[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            lidarData[i].distance = float.Parse(parts[0]);
            lidarData[i].obstacleName = parts[1];
        }
    }

    void ReadRGBData()
    {
        // Read RGB data from file
        string[] lines = File.ReadAllLines(rgbDataFileName);
        rgbData = new RGBPixel[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            rgbData[i].r = float.Parse(parts[0]);
            rgbData[i].g = float.Parse(parts[1]);
            rgbData[i].b = float.Parse(parts[2]);
        }
    }

    void PerformFusion()
    {
        // Define color ranges (example ranges)
        float redThreshold = 0.9f;
        float greenThreshold = 0.9f;
        float blueThreshold = 0.9f;

        // Iterate through LiDAR data to detect obstacles and determine their colors
        for (int i = 0; i < lidarData.Length; i++)
        {
            if (lidarData[i].distance < obstacleThreshold)
            {
                // Find corresponding RGB pixel based on LiDAR point's position
                // (Assuming some mapping between LiDAR data and RGB data)
                int rgbIndex = MapLiDARToRGB(i);

                // Determine obstacle color based on RGB data
                string obstacleColor = ClassifyColor(rgbData[rgbIndex]);

                // Output obstacle distance and color
                Debug.Log("Obstacle detected at distance " + lidarData[i].distance + " with color " + obstacleColor);
            }
        }
    }

    int MapLiDARToRGB(int lidarIndex)
    {
        // Implement mapping logic to find corresponding RGB pixel based on LiDAR point's position
        // You may need to interpolate or approximate based on LiDAR point's position
        // For simplicity, let's assume a direct mapping for demonstration
        return lidarIndex;
    }

    string ClassifyColor(RGBPixel pixel)
    {
        // Define color ranges and classify based on RGB values
        if (pixel.r >= 0.5f && pixel.g < 0.5f && pixel.b < 0.5f)
        {
            return "Red";
        }
        else if (pixel.r < 0.5f && pixel.g >= 0.5f && pixel.b < 0.5f)
        {
            return "Green";
        }
        else if (pixel.r < 0.5f && pixel.g < 0.5f && pixel.b >= 0.5f)
        {
            return "Blue";
        }
        else
        {
            return "Unknown"; // Or you can further classify based on other colors
        }
    }

}
