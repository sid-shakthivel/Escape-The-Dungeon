using System.Collections;
using UnityEngine;

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
    public float ArrowSpeed;
    public Joystick joystick;

    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private float ArrowCount = 10;
    private float Hearts = 10;
    private float ArrowPower = 10;
    private ePlayerState PlayerState = ePlayerState.Idle;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Vector3 MovementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //PlayerAnimator.SetFloat("HorizontalSpeed", Input.GetAxis("Horizontal") * Speed);
        //PlayerAnimator.SetFloat("VerticalSpeed", Input.GetAxis("Vertical") * Speed);

        Vector3 MovementInput = new Vector3(joystick.Horizontal, joystick.Vertical, 0);

        if (joystick.Vertical > 0.5f || joystick.Vertical < -0.5f)
            PlayerAnimator.SetFloat("VerticalSpeed", joystick.Vertical * Speed);
        else
            PlayerAnimator.SetFloat("VerticalSpeed", 0);

        if (joystick.Horizontal > 0.5f || joystick.Horizontal < -0.5f)
            PlayerAnimator.SetFloat("HorizontalSpeed", joystick.Horizontal * Speed);
        else
            PlayerAnimator.SetFloat("HorizontalSpeed", 0);

        PlayerAnimator.SetBool("IsAttack", false);

        SetCurrentState();

        PlayerRigidbody.MovePosition(transform.position + MovementInput * Time.deltaTime * Speed);

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (Hit == true && Hit.collider.CompareTag("Crate"))
            {
                GameObject ClosestCrate = Helper.FindClosestGameObject("Crate", transform.position);
                Crate CrateScript = ClosestCrate.GetComponent<Crate>();
                CrateScript.LootCrate();
            }
        }

        if (Input.GetButtonDown("Fire"))
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
    }

    private void SetCurrentState()
    {
        if (PlayerAnimator.GetFloat("HorizontalSpeed") < -0.01) PlayerState = ePlayerState.Left;
        if (PlayerAnimator.GetFloat("HorizontalSpeed") > 0.01) PlayerState = ePlayerState.Right;
        if (PlayerAnimator.GetFloat("VerticalSpeed") < -0.01) PlayerState = ePlayerState.Down;
        if (PlayerAnimator.GetFloat("VerticalSpeed") > 0.01) PlayerState = ePlayerState.Up;
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
