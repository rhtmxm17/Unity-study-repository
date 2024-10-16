using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Launcher : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Rigidbody projectile;
    [SerializeField] float force;

    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
    }

    private void Start()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // 직접 잡은 경우에만 허용
        if (args.interactorObject is XRDirectInteractor)
        {
            interactable.activated.AddListener(Fire);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        interactable.activated.RemoveListener(Fire);
    }

    public void Fire(ActivateEventArgs _)
    {
        Rigidbody instance = Instantiate(projectile, muzzle.position, muzzle.rotation);
        instance.AddForce(force * muzzle.forward, ForceMode.Impulse);
    }
}
