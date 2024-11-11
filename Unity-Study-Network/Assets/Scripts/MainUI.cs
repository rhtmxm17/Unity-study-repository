using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] TMP_Text announceUI;
    [SerializeField] float announceTimer = 2.5f;

    private Coroutine announceRoutine;

    public void Announce(string text)
    {
        announceUI.text = text;
        if (announceRoutine != null)
        {
            StopCoroutine(announceRoutine);
        }
        announceRoutine = StartCoroutine(ShowAnnounce(announceTimer));
    }

    private IEnumerator ShowAnnounce(float time)
    {
        announceUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        announceUI.gameObject.SetActive(false);
        announceRoutine = null;
    }
}
