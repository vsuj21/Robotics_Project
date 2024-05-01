using UnityEngine;
using System.IO;

public class LiDAR : MonoBehaviour
{
    public int imageWidth = 1920; // Width of the camera image
    public int imageHeight = 1080; // Height of the camera image
    public string lidarDataFileName = "lidar_data.txt"; // File to save LiDAR-like data

    private StreamWriter writer;
    private Camera mainCamera;

    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
    }

    public void PerformLiDARScan()
    {
        // Open the file for writing LiDAR-like data
        writer = new StreamWriter(lidarDataFileName);

        // Iterate over all pixels in the camera image
        for (int y = 0; y < imageHeight; y++)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                // Convert pixel coordinates to viewport coordinates (normalized screen coordinates)
                Vector3 viewportPos = new Vector3((float)x / imageWidth, (float)y / imageHeight, 0);

                // Cast a ray from the camera through the current pixel
                Ray ray = mainCamera.ViewportPointToRay(viewportPos);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Process LiDAR-like data (e.g., save hit point position to file)
                    Vector3 hitPoint = hit.point;
                    writer.WriteLine(hitPoint.x + "," + hitPoint.y + "," + hitPoint.z);
                }
                else
                {
                    // If no hit, represent as infinity
                    writer.WriteLine("inf,inf,inf");
                }
            }
        }

        // Close the file when LiDAR scan is complete
        writer.Close();
    }
}
