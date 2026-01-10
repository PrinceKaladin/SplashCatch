using UnityEngine;

public class FishMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float smooth = 3f;

    [Header("Wave Motion")]
    public float waveHeight = 0.3f;
    public float waveSpeed = 2f;

    public float killX = 12f;

    int dir = 1;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void InitDirection(int direction)
    {
        dir = direction >= 0 ? 1 : -1;

        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += dir * moveSpeed * Time.deltaTime;

        float wave = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        pos.y = startPos.y + wave;

        transform.position = pos;

        if (dir == 1 && transform.position.x > killX) Destroy(gameObject);
        if (dir == -1 && transform.position.x < -killX) Destroy(gameObject);
    }

}
