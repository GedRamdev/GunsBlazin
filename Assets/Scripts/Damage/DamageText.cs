using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textObject;
    [SerializeField]
    private float expiryTime;

    private float time = 0.0f;

    public void SetText(string text)
    {
        this.textObject.text = text;
    }

    private void Update()
    {
        time += Time.deltaTime;
        this.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        if(time >= expiryTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
