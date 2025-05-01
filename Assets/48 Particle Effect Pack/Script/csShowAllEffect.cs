using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class csShowAllEffect : MonoBehaviour
{
    public string[] EffectNames;
    public string[] Effect2Names;
    public Transform[] Effect;
    public Text Text1; // تغییر از GUIText به UI.Text
    int i = 0;
    int a = 0;

    void Start()
    {
        Instantiate(Effect[i], new Vector3(0, 5, 0), Quaternion.identity);
    }

    void Update()
    {
        Text1.text = (i + 1) + ": " + EffectNames[i];

        if (Input.GetKeyDown(KeyCode.Z))
        {
            i = (i <= 0) ? 99 : i - 1;
            ShowEffect();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            i = (i < 99) ? i + 1 : 0;
            ShowEffect();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowEffect();
        }
    }

    void ShowEffect()
    {
        for (a = 0; a < Effect2Names.Length; a++)
        {
            if (EffectNames[i] == Effect2Names[a])
            {
                Instantiate(Effect[i], new Vector3(0, 0.01f, 0), Quaternion.identity);
                return;
            }
        }
        Instantiate(Effect[i], new Vector3(0, 5, 0), Quaternion.identity);
    }
}
