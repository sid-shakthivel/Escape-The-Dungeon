using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text HeartText;
    public Text BulletCountText;

    private Player PlayerScript;

    private void Awake()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<Player>();
    }

    private void Update()
    {
        HeartText.text = "Hearts: " + PlayerScript.GetHearts();
        BulletCountText.text = "Arrows: " + PlayerScript.GetArrowCount();
    }
}
