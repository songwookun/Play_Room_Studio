using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, MP, Coin } // Coin 추가

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
                {
                    int totalSeconds = (int)GameManager.Instance.gameTime;
                    int minutes = totalSeconds / 60;
                    int seconds = totalSeconds % 60;
                    mytext.text = $"{minutes:00}:{seconds:00}";
                }
                break;

            case InfoType.Health:
                if (myslider != null)
                {
                    float curHealth = GameManager.Instance.health;
                    float maxHealth = GameManager.Instance.maxHealth;
                    myslider.value = curHealth / maxHealth;
                }
                break;

            case InfoType.Coin: // 코인 수 표시
                if (mytext != null)
                    mytext.text = GameManager.Instance.collectedCoins.ToString();
                break;
        }
    }
}
