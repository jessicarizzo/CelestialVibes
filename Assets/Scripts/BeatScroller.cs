using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float m_beatTempo;
    private const float c_secondsInMinute = 60f;
    public bool hasStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        m_beatTempo /= c_secondsInMinute;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            transform.position -= new Vector3(0f, m_beatTempo * Time.deltaTime, 0f);
        }
    }
}
