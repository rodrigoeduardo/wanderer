using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInputSignalScript : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
        Invoke(nameof(Activate), 2f);
    }

    public void Activate() {
        text.enabled = true;
        StopAllCoroutines();
        StartCoroutine(Blinking());
    }

    public void Deactivate() {
        text.enabled = false;
    }

    IEnumerator Blinking() {
        text.text = ")";
        yield return new WaitForSeconds(1);
        text.text += ")";
        yield return new WaitForSeconds(1);
        text.text += ")";
        yield return new WaitForSeconds(1);

        if (text.enabled) StartCoroutine(Blinking());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
