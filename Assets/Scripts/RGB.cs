using UnityEngine;
using System.IO;

public class RGB : MonoBehaviour
{
    public Camera mainCamera;
    public RenderTexture renderTexture;
    public string captureFileName = "camera_capture.png";

    public void CaptureImage()
    {
        Debug.Log("startedcapturing");
        // Ensure the main camera is set
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is not assigned!");
            return;
        }

        // Ensure the render texture is set
        if (renderTexture == null)
        {
            Debug.LogError("Render texture is not assigned!");
            return;
        }

        // Set the render texture as the target for rendering
        mainCamera.targetTexture = renderTexture;

        // Render the scene from the camera's perspective
        mainCamera.Render();

        // Read pixel data from the render texture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Encode the texture to PNG format
        byte[] bytes = texture.EncodeToPNG();

        // Save the PNG image to a file
        File.WriteAllBytes(captureFileName, bytes);

        // Reset the target texture
        mainCamera.targetTexture = null;
    }
}
