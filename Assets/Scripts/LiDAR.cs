using UnityEngine;
using System.IO;

public class LiDAR : MonoBehaviour
{
    public float maxDistance = 10f; // Maximum detection range of LiDAR
    public int numberOfRays = 360; // Number of rays to cast for 360-degree coverage
    public float angleIncrement = 1f; // Increment angle between each ray
    public string lidarDataFileName = "lidar_data.txt"; // File to save LiDAR data

    private StreamWriter writer;
    private bool hasScanned = false; // Flag to track if LiDAR has scanned

    void Start()
    {
        // Check if LiDAR has already scanned
        if (!hasScanned)
        {
            // Open a file for writing LiDAR data
            writer = new StreamWriter(lidarDataFileName);

            // Perform LiDAR scan
            PerformLiDARScan();

            // Set flag to true to prevent further scans
            hasScanned = true;
        }
    }

    void PerformLiDARScan()
    {
        // Iterate over the specified number of rays
        for (float angle = 0; angle < 360; angle += angleIncrement)
        {
            // Calculate ray direction based on current angle
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, maxDistance))
            {
                // Process LiDAR data (e.g., save distance to file)
                writer.WriteLine(hit.distance + "," + hit.collider.gameObject.name); // Also save obstacle's name
            }
        }

        // Close the file when LiDAR scan is complete
        writer.Close();
    }
}
