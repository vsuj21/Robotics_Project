using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Fusion : MonoBehaviour
{
    public RenderTexture capturedRenderTexture;
    public string lidarDataFileName = "lidar_data.txt";
    public string fusionOutputFileName = "fusion_output.png";

    public int sampleRate = 10; // Adjust the sampling rate to control point density

    public void StartFusion()
    {
        // Ensure the render texture is set
        if (capturedRenderTexture == null)
        {
            Debug.LogError("Render texture is not assigned!");
            return;
        }

        // Load LiDAR data from file
        string[] lidarLines = File.ReadAllLines(lidarDataFileName);

        // Process LiDAR data and convert to a list of Vector3 points
        List<Vector3> lidarPoints = new List<Vector3>();
        foreach (string line in lidarLines)
        {
            if (line.StartsWith("inf"))
                continue;

            string[] parts = line.Split(',');
            Vector3 lidarPoint = new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            lidarPoints.Add(lidarPoint);
        }

        // Create a new Texture2D with the dimensions of the render texture
        Texture2D capturedImage = new Texture2D(capturedRenderTexture.width, capturedRenderTexture.height);

        // Set the active render texture and read the pixels from it
        RenderTexture.active = capturedRenderTexture;
        capturedImage.ReadPixels(new Rect(0, 0, capturedRenderTexture.width, capturedRenderTexture.height), 0, 0);
        capturedImage.Apply();

        // Reset the active render texture
        RenderTexture.active = null;

        // Project LiDAR points onto image with sampling
        foreach (Vector3 lidarPoint in lidarPoints)
        {
            // Adjust LiDAR point for offset
            Vector3 adjustedPoint = transform.TransformPoint(lidarPoint);

            // Convert adjusted 3D LiDAR point to world coordinates
            Vector3 worldPoint = Camera.main.WorldToScreenPoint(adjustedPoint);

            // Overlay LiDAR points onto image
            if (IsPointWithinImage(worldPoint))
            {
                // Calculate distance from LiDAR sensor (assuming 3D Euclidean distance)
                float distance = Vector3.Distance(adjustedPoint, Camera.main.transform.position);

                // Assign color based on distance
                Color pointColor = DistanceToColor(distance);

                // Set pixel color
                capturedImage.SetPixel((int)worldPoint.x, (int)worldPoint.y, pointColor);
            }
        }

        // Apply changes to the image
        capturedImage.Apply();

        // Save the fused output image
        byte[] bytes = capturedImage.EncodeToPNG();
        File.WriteAllBytes(fusionOutputFileName, bytes);
    }

    bool IsPointWithinImage(Vector3 point)
    {
        // Check if the point is within the bounds of the image
        return point.x >= 0 && point.x < capturedRenderTexture.width && point.y >= 0 && point.y < capturedRenderTexture.height;
    }

    Color DistanceToColor(float distance)
    {
        // Adjust the color assignment based on distance
        if (distance < 5)
        {
            return Color.red; // Close objects
        }
        else if (distance < 10)
        {
            return Color.green; // Mid-range objects
        }
        else
        {
            return Color.blue; // Far objects
        }
    }
}
