using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    // To switch the images of the button sprite
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    // We want buttons to react to keypress
    public KeyCode keyToPress;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.sprite = defaultImage;
        }
    }
}
