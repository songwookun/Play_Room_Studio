using UnityEngine;
using UnityEngine.UI;

public class ExitOnBackButton : MonoBehaviour
{
    public GameObject exitPopup;   // ���� Ȯ�� �˾� (��Ȱ��ȭ ���·� �����ؾ� ��)

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (exitPopup.activeSelf)
                {
                    exitPopup.SetActive(false);  // �̹� ���� ������ �ݱ�
                }
                else
                {
                    exitPopup.SetActive(true);   // �ƴϸ� ����
                }
            }
        }
    }

    // ��ư�� ������ �Լ���
    public void OnConfirmExit()
    {
#if UNITY_EDITOR
        // �����Ϳ����� Play ��� ����
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    // PC ����� �������Ͽ��� ����
    Application.Quit();
#elif UNITY_ANDROID
    // �ȵ���̵� �� ����
    Application.Quit();
#endif
    }

    public void OnCancelExit()
    {
        exitPopup.SetActive(false);
    }
}
