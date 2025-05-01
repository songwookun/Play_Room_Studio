using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public int noDamage = 0; // 1�̸� ���� ���

    public static CheatManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsNoDamageActive()
    {
        return noDamage == 1;
    }
}
