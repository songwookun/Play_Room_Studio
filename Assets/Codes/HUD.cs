using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text mytext;
    Slider myslider;

    void Awake()
    {
        mytext = GetComponent<Text>();
        myslider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                if (GameManager.Instance != null)
                {
                    float curExp = GameManager.Instance.exp;
                    float maxExp = GameManager.Instance.GetNextExp(GameManager.Instance.level);

                    if (myslider != null)
                        myslider.value = curExp / maxExp;
                    if (mytext != null)
                        mytext.text = $"{curExp} / {maxExp}";
                }
                break;

            case InfoType.Level:
                if (mytext != null) 
                    mytext.text = string.Format("Lv.{0:F0}", GameManager.Instance.level);
                break;

            case InfoType.Kill:
                if (mytext != null)  
                    mytext.text = string.Format("{0:F0}", GameManager.Instance.Kill);
                break;

            case InfoType.Time:
                if (mytext != null)  
                    mytext.text = $"{GameManager.Instance.gameTime:F1}s"; 
                break;

            case InfoType.Health:
                if (mytext != null)  
                    mytext.text = "100%"; 
                break;
        }
    }
}
