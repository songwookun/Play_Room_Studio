using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneCoinLoader : MonoBehaviour
{
    public Text coinText;

    private void Start()
    {
        if (coinText == null)
        {
            // �ڵ����� "coin Text" ������Ʈ ã��
            GameObject coinObj = GameObject.Find("coin Text");
            if (coinObj != null)
                coinText = coinObj.GetComponent<Text>();
        }

        string path = Path.Combine(Application.persistentDataPath, "coin_data.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CoinData data = JsonUtility.FromJson<CoinData>(json);
            coinText.text = data.totalCoins.ToString();
            Debug.Log($"[StartScene] ����� ���� ��: {data.totalCoins}");
        }
        else
        {
            coinText.text = "0";
            Debug.LogWarning("���� JSON ������ �������� �ʾ� 0���� ǥ�õ�");
        }
    }
}
