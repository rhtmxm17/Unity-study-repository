using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollectingStage : MonoBehaviour
{
    public UnityEvent OnClear;

    [SerializeField] CollectingTarget[] goals;

    [SerializeField]
    private int remainningGoalCount;

    private void Start()
    {
        foreach (CollectingTarget goal in goals)
        {
            remainningGoalCount++;
            goal.OnCollected += () =>
            {
                remainningGoalCount--;
                if (remainningGoalCount == 0)
                    OnClear.Invoke();
            };
        }

        var input = PlayerInput.GetPlayerByIndex(0);
        input.actions["R"].performed += context =>
        {
            SceneManager.LoadScene("Platformer");
        };
    }
}
