using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HookControl : MonoBehaviour
{
    [Header("References")]
    public Transform rodTip;          // точка выхода лески
    public LineRenderer line;         // леска
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip fishClip;
    public AudioClip sharkClip;

    [Header("Cast & Limits")]
    public float castDepth = -3.5f;   // глубина заброса
    public float minY = -4.0f;        // нижний предел
    public float maxY = 0.0f;         // верхний предел

    [Header("Smooth Movement")]
    [Tooltip("Меньше = плавнее")]
    public float riseSmoothTime = 0.15f;
    public float fallSmoothTime = 0.25f;

    float targetY;
    float velocityY;

    bool isCast;
    bool isHolding;

    void Start()
    {
        PlayerPrefs.SetInt("score",0);
        ResetHook();
        SetupLine();
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver)
            return;

        HandleInput();
        MoveHookSmooth();
        UpdateLine();
    }

    // ================= INPUT =================
    void HandleInput()
    {
        // Мобильный ввод
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (!isCast) Cast();
                isHolding = true;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                isHolding = false;
            }

            return;
        }

        // Мышь (для редактора)
        if (Input.GetMouseButtonDown(0))
        {
            if (!isCast) Cast();
            isHolding = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }
    }

    // ================= CAST =================
    void Cast()
    {
        isCast = true;

        targetY = rodTip.position.y + castDepth;
        targetY = Mathf.Clamp(targetY, minY, maxY);

        transform.position = new Vector3(
            rodTip.position.x,
            targetY,
            -5
        );
    }

    // ================= MOVEMENT =================
    void MoveHookSmooth()
    {
        if (!isCast) return;

        // цель
        targetY = isHolding ? maxY : minY;

        float smoothTime = isHolding ? riseSmoothTime : fallSmoothTime;

        float newY = Mathf.SmoothDamp(
            transform.position.y,
            targetY,
            ref velocityY,
            smoothTime
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
           -5
        );
    }

    // ================= LINE =================
    void SetupLine()
    {
        if (!line) return;

        line.positionCount = 2;
        line.useWorldSpace = true;
    }

    void UpdateLine()
    {
        if (!line || !rodTip) return;

        line.SetPosition(0, rodTip.position);
        line.SetPosition(1, transform.position);
    }

    // ================= COLLISIONS =================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.I != null && GameManager.I.IsGameOver)
            return;

        if (other.CompareTag("Fish"))
        {
            GameManager.I.AddScore(10);
            PlaySound(fishClip);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shark"))
        {
            GameManager.I.AddScore(-10);
            PlaySound(sharkClip);
            Destroy(other.gameObject);
        }
    }

    // ================= UTILS =================
    void PlaySound(AudioClip clip)
    {
        if (!audioSource || !clip) return;
        audioSource.PlayOneShot(clip);
    }

    void ResetHook()
    {
        isCast = false;
        isHolding = false;
        velocityY = 0f;

        if (rodTip)
            transform.position = rodTip.position;
    }
}
