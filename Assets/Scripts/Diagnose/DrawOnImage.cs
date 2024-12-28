using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;



public class DrawOnRawImage : MonoBehaviour
{
    public GameObject Brush;
    public Text infoText; // Reference to the text field to display information

    public GameObject objectToDestroy; // The object to destroy upon saving
    public float BrushSize = 0.1f;
    public RawImage whiteImage;
    public Button writeButton; // Button for writing
    public Button eraseButton; // Button for erasing
    public Button clearButton; // Button for clearing the drawing
    public Button saveButton; // Button for saving the image
    private Texture2D maskTexture;
    private Texture2D originalTexture;
    private RectTransform rectTransform;
    private Vector2 lastDrawPoint; // Last point where drawing or erasing occurred
    public float sensitivity = 0.1f; // Sensitivity for drawing speed adjustment
    private bool isWriting = true; // Flag to indicate whether writing or erasing is active
    private float customDeltaTime;
 public PhraseGenerator phraseGenrator;
    public GameObject nextPrefabToInstantiate; // Serialized field to assign the prefab for instantiation
    void Start()
    {

        rectTransform = whiteImage.rectTransform;
        // Create the initial mask texture
        CreateMaskTexture();
         Time.timeScale=0;
        customDeltaTime = Time.unscaledDeltaTime; // Initialize custom deltaTime

        // Save the original texture
        originalTexture = new Texture2D(maskTexture.width, maskTexture.height);
        originalTexture.SetPixels(maskTexture.GetPixels());
        originalTexture.Apply();

        // Assign onClick events to the buttons
        writeButton.onClick.AddListener(SetWritingMode);
        eraseButton.onClick.AddListener(SetErasingMode);
        clearButton.onClick.AddListener(ClearDrawing);
        saveButton.onClick.AddListener(SaveImage);
    }
void Awake()
{
            PlayerPrefs.SetInt("flag", 0);

}
    // Update is called once per frame
    void Update()
    {         customDeltaTime = Time.unscaledDeltaTime; // Update custom deltaTime

        if (Input.GetMouseButton(0))
        {
            if (isWriting)
            {
                // Draw with the brush
                DrawWithBrush();
            }
            else
            {
                // Erase with the eraser
                Erase();
            }
        }
        else
        {
            // Reset last draw point when mouse button is released
            lastDrawPoint = Vector2.zero;
        }
    }

    void CreateMaskTexture()
    {
        // Create a mask texture with the same dimensions as the RawImage
        maskTexture = new Texture2D((int)rectTransform.rect.width, (int)rectTransform.rect.height);
        // Initialize the mask texture with all white pixels
        for (int x = 0; x < maskTexture.width; x++)
        {
            for (int y = 0; y < maskTexture.height; y++)
            {
                maskTexture.SetPixel(x, y, Color.white);
            }
        }
        maskTexture.Apply();

        // Apply the mask texture to the RawImage
        whiteImage.texture = maskTexture;
    }

    void DrawWithBrush()
    {
        // Sample points along the mouse movement path
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        if (lastDrawPoint == Vector2.zero || Vector2.Distance(localPoint, lastDrawPoint) > BrushSize * sensitivity)
        {
            DrawOnMask(localPoint, Color.black);
            lastDrawPoint = localPoint;
        }
    }

    void Erase()
    {
        // Sample points along the mouse movement path
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);
        if (lastDrawPoint == Vector2.zero || Vector2.Distance(localPoint, lastDrawPoint) > BrushSize * sensitivity)
        {
            DrawOnMask(localPoint, Color.white);
            lastDrawPoint = localPoint;
        }
    }

    void DrawOnMask(Vector2 position, Color color)
    {
        // Convert position to UV coordinates
        Vector2 uvCoords = new Vector2((position.x + rectTransform.rect.width * 0.5f) / rectTransform.rect.width,
                                       (position.y + rectTransform.rect.height * 0.5f) / rectTransform.rect.height);

        // Convert UV coordinates to pixel coordinates
        int pixelX = Mathf.FloorToInt(uvCoords.x * maskTexture.width);
        int pixelY = Mathf.FloorToInt(uvCoords.y * maskTexture.height);

        // Calculate the radius of the circular brush
        int radius = Mathf.CeilToInt(BrushSize);

        // Draw on the mask texture within the circular area
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                // Calculate the distance from the current pixel to the center
                float distance = Mathf.Sqrt(x * x + y * y);

                // Check if the current pixel is within the circular brush area
                if (distance <= BrushSize)
                {
                    int drawX = pixelX + x;
                    int drawY = pixelY + y;

                    // Ensure the pixel is within the bounds of the mask texture
                    if (drawX >= 0 && drawX < maskTexture.width && drawY >= 0 && drawY < maskTexture.height)
                    {
                        maskTexture.SetPixel(drawX, drawY, color);
                    }
                }
            }
        }

        // Apply changes to the mask texture
        maskTexture.Apply();
    }

    void ClearDrawing()
    {
        // Restore the original texture
        maskTexture.SetPixels(originalTexture.GetPixels());
        maskTexture.Apply();
    }

   void SaveImage()
{
     if (!HasDrawnOnMask())
        {
            // If the mask texture is unchanged, display a message in the text field
            infoText.text = "Please draw something before saving!";
            return;
        }
    // Encode the mask texture to PNG
    byte[] bytes = maskTexture.EncodeToPNG();

    // Save the PNG to disk
    string filePath = Application.persistentDataPath + "/savedImage.png";
    File.WriteAllBytes(filePath, bytes);
    Debug.Log("Saved image to: " + filePath);

    // Set flag to 1 in PlayerPrefs
    PlayerPrefs.SetInt("flag", 1);
    Debug.Log("Flag set to 1: " + PlayerPrefs.GetInt("flag")); // Debug the value of flag

    // Upload the image to the server
    StartCoroutine(UploadImage(filePath));

    // Destroy an object in the scene upon saving
    DestroyObjectInScene();
}


IEnumerator UploadImage(string imagePath)
{
    // Create a form for sending the image data
    WWWForm form = new WWWForm();
    form.AddBinaryData("image", File.ReadAllBytes(imagePath), "savedImage.png", "image/png");

    // Get the current phrase from your PhraseGenerator
    string currentPhrase = phraseGenrator.GetCurrentPhrase();

    // Log currentPhrase to check its value
    if (currentPhrase != null)
    {
        Debug.Log("Current Phrase: " + currentPhrase);

        // Add the current phrase to the form data
        form.AddField("currentText", currentPhrase); // Assuming 'currentText' is the parameter name expected by your server

        // Construct the URL
        string url = "http://localhost:3000/ocr/";

        // Send the POST request to the server
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // Send the request
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to upload image: " + www.error);
            }
            else
            {
                Debug.Log("Image uploaded successfully!");
                // Handle the response if needed
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }
    else
    {
        Debug.LogError("Current Phrase is null!");
    }
}



    public void SetWritingMode()
    {
        isWriting = true; // Set writing mode
    }

    public void SetErasingMode()
    {
        isWriting = false; // Set erasing mode
    }

    void DestroyObjectInScene()
    {   Time.timeScale=1;
        // Destroy the assigned object in the scene upon saving
        if (objectToDestroy != null)
        {

            Destroy(objectToDestroy);
            Debug.Log("Object destroyed from the scene upon saving.");
             if (nextPrefabToInstantiate != null)
        {
            Instantiate(nextPrefabToInstantiate, objectToDestroy.transform.position, objectToDestroy.transform.rotation);
        }
        else
        {
            Debug.LogWarning("No next prefab assigned to instantiate.");
        }
        }
        else
        {
            Debug.LogWarning("No object assigned to destroy upon saving.");
        }
    }
    bool HasDrawnOnMask()
    {
        // Check if any pixel in the mask texture is not white
        Color[] pixels = maskTexture.GetPixels();
        foreach (Color pixel in pixels)
        {
            if (pixel != Color.white)
            {
                return true;
            }
        }
        return false;
    }
}