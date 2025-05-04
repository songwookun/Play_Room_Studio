using UnityEngine;
using UnityEngine.UI;

public class ExitOnBackButton : MonoBehaviour
{
    public GameObject exitPopup;   // 종료 확인 팝업 (비활성화 상태로 시작해야 함)

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (exitPopup.activeSelf)
                {
                    exitPopup.SetActive(false);  // 이미 열려 있으면 닫기
                }
                else
                {
                    exitPopup.SetActive(true);   // 아니면 열기
                }
            }
        }
    }

    // 버튼에 연결할 함수들
    public void OnConfirmExit()
    {
#if UNITY_EDITOR
        // 에디터에서는 Play 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    // PC 빌드된 실행파일에서 종료
    Application.Quit();
#elif UNITY_ANDROID
    // 안드로이드 앱 종료
    Application.Quit();
#endif
    }

    public void OnCancelExit()
    {
        exitPopup.SetActive(false);
    }
}
