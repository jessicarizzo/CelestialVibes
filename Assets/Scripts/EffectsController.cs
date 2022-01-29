using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public float lifeTime = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);   
    }
}
