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
            // 자동으로 "coin Text" 오브젝트 찾기
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
            Debug.Log($"[StartScene] 저장된 코인 수: {data.totalCoins}");
        }
        else
        {
            coinText.text = "0";
            Debug.LogWarning("코인 JSON 파일이 존재하지 않아 0으로 표시됨");
        }
    }
}
