using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    enum ePlayerState
    {
        Idle,
        Up,
        Down,
        Right,
        Left
    }

    public Rigidbody2D Arrow;
    public float Speed;

    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private float ArrowCount = 10;
    private float Hearts = 10;
    private float ArrowPower = 10;
    private ePlayerState PlayerState = ePlayerState.Idle;
    private Touch touch;
    private float TouchDuration;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 MovementInput;

        //MovementInput = new Vector3(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"), 0);
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        PlayerAnimator.SetFloat("HorizontalSpeed", MovementInput.x * Speed);
        PlayerAnimator.SetFloat("VerticalSpeed", MovementInput.y * Speed);

        PlayerAnimator.SetBool("IsAttack", false);

        SetCurrentState();

        PlayerRigidbody.MovePosition(transform.position + MovementInput * Time.deltaTime * Speed);

        if (Input.touchCount > 0)
        {
            TouchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && TouchDuration < 0.2f)
                StartCoroutine("SingleOrDoubleTap");
        }
        else
            TouchDuration = 0f;

        if (Input.GetMouseButtonUp(0))
            CheckForChest(Input.mousePosition);

        if (Input.GetButtonDown("Fire"))
            FireArrow();
    }

    IEnumerator SingleOrDoubleTap()
    {
        yield return new WaitForSeconds(0.3f);
        if (touch.tapCount == 1)
            FireArrow();
        else
        {
            StopCoroutine("SingleOrDoubleTap");
            CheckForChest(touch.position);
        }
    }

    private void SetCurrentState()
    {
        if (PlayerAnimator.GetFloat("HorizontalSpeed") < -0.01) PlayerState = ePlayerState.Left;
        if (PlayerAnimator.GetFloat("HorizontalSpeed") > 0.01) PlayerState = ePlayerState.Right;
        if (PlayerAnimator.GetFloat("VerticalSpeed") < -0.01) PlayerState = ePlayerState.Down;
        if (PlayerAnimator.GetFloat("VerticalSpeed") > 0.01) PlayerState = ePlayerState.Up;
    }

    private void CheckForChest (Vector3 Position)
    {
        RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Position));

        if (Hit == true && Hit.collider.CompareTag("Crate"))
        {
            Crate CrateScript = Hit.collider.GetComponent<Crate>();
            //CrateScript.LootCrate();
            StartCoroutine(CrateScript.LootCrate());
        }
    }

    private void FireArrow()
    {
        PlayerAnimator.SetBool("IsAttack", true);
        Rigidbody2D InstaniatedArrow = Instantiate(Arrow, transform.position, Quaternion.identity);

        switch (PlayerState)
        {
            case ePlayerState.Up:
                InstaniatedArrow.velocity = Speed * Vector2.up;
                break;
            case ePlayerState.Right:
                InstaniatedArrow.velocity = Speed * Vector2.right;
                break;
            case ePlayerState.Left:
                InstaniatedArrow.velocity = Speed * Vector2.left;
                break;
            default:
                InstaniatedArrow.velocity = Speed * Vector2.down;
                break;
        }
    }

    public float GetArrowCount()
    {
        return ArrowCount;
    }

    public float GetHearts()
    {
        return Hearts;
    }

    public float GetArrowPower()
    {
        return ArrowPower;
    }

    public void SetArrowCount(float NewArrowCount)
    {
        ArrowCount = NewArrowCount;
    }

    public void SetHearts(float NewHealth)
    {
        Hearts = NewHealth;
    }

    public void SetArrowPower(float NewArrowPower)
    {
        ArrowPower = NewArrowPower;
    }
}
