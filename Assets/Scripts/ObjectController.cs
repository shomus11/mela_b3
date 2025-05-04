using DG.Tweening;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public bool canRot = true;
    public float rotationSpeed = 100f;
    public Animator animator;
    private Vector2 lastTouchPos;
    private bool isDragging = false;
    private Vector3 startPos;
    private Vector3 startRot;
    Sequence display;
    float baseAnimationDuration = .25f;
    private void Start()
    {

    }
    public void Init()
    {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.DOScale(0.75f, 0);
    }

    public void SetPosition()
    {
        gameObject.transform.position = startPos;
        gameObject.transform.rotation = Quaternion.Euler(startRot);
        gameObject.transform.DOScale(0.75f, 0);
    }
    void Update()
    {
        if (canRot)
        {

            float horizontal = Input.GetAxis("Horizontal");
            transform.Rotate(0f, horizontal * 100f * Time.deltaTime, 0f);
            // Mobile Touch Input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        lastTouchPos = touch.position;
                        isDragging = true;
                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            Vector2 delta = touch.deltaPosition;
                            RotateObject(delta);
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isDragging = false;
                        break;
                }
            }
            // Mouse Input (for PC editor testing)
            else if (Input.GetMouseButtonDown(0))
            {
                lastTouchPos = Input.mousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
                lastTouchPos = Input.mousePosition;
                RotateObject(delta);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    void RotateObject(Vector2 delta)
    {
        float x = delta.x * rotationSpeed * Time.deltaTime;
        float y = delta.y * rotationSpeed * Time.deltaTime;

        // Rotate around Y axis (horizontal swipe) and X axis (vertical swipe)
        transform.Rotate(Vector3.up, -x, Space.World);
        transform.Rotate(Vector3.right, y, Space.World);
    }
    public void SetAnimator(bool set)
    {
        animator.enabled = set;
    }

    public void RotateObject(bool set)
    {

        if (set)
        {
            display = DOTween.Sequence();
            float totalAnimationDuration = 0;
            display.Insert(0, gameObject.transform.DOLocalRotate(
               new Vector3(0, 180, 360), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1));
            display.Insert(0, gameObject.transform.DOLocalMoveZ(startPos.z - 1, baseAnimationDuration * 24).From(startPos.z + 3).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear));
            display.Insert(0, gameObject.transform.DOScale(Vector3.one * .75f, baseAnimationDuration * 24).From(Vector3.one * .25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear));
        }
        else
        {
            display.Kill();
        }
    }
}
