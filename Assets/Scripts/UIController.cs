using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text TimeText;
    public Text CoinText;
    public Text ArrowText;
    public List<GameObject> Hearts;

    private Player PlayerScript;

    private void Awake()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<Player>();
    }

    private void Update()
    {
        TimeText.text = "" + Mathf.Floor(Time.realtimeSinceStartup);
        CoinText.text = "" + PlayerScript.PlayerCoinCount;
        ArrowText.text = "" + PlayerScript.EntityProjectileCount;
    }

    public void AddHeart()
    {
        foreach (GameObject Heart in Hearts)
        {
            if (Heart.activeSelf == false)
            {
                Heart.SetActive(true);
                break;
            }
        }
    }

    public void RemoveHeart()
    {
        Hearts.ElementAt((int)PlayerScript.EntityHeartCount).SetActive(false);
    }
}
