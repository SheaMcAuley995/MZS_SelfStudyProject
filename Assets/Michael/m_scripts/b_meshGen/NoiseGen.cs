
using UnityEngine;

public class NoiseGen : MonoBehaviour {


    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    public float offsetX = 100;
    public float offsetY = 100;


    void Start()
    {
        offsetX = Random.Range(0, 10000);
        offsetY = Random.Range(0, 10000);
    }
	void Update ()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenTexture();
	}

    private Texture2D GenTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
        
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
         
    }
}
