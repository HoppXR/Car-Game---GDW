using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Camera minimapCamera;
    public RawImage minimap;

    void OnGUI()
    {
        // Minimap Texture
        RenderTexture.active = minimapCamera.targetTexture;
        minimapCamera.Render();

        Texture2D texture = new Texture2D(minimapCamera.targetTexture.width, minimapCamera.targetTexture.height);
        texture.ReadPixels(new Rect(0, 0, minimapCamera.targetTexture.width, minimapCamera.targetTexture.height), 0, 0);
        texture.Apply();

        minimap.texture = texture;
        RenderTexture.active = null;
    }
}
