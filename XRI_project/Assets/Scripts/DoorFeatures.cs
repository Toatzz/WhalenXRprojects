using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorFeatures : CoreFeatures
{
    //Door Configuration - pivot information, open door state, max angle, Reverse,speed

    [Header("Door Configurations")]
    
    [SerializeField]
    private Transform doorPivot; //controls pivot

    [SerializeField]
    private float maxAngle = 90.0f; //Maybe <90 will be wanted

    [SerializeField]
    private bool reverseAngleDirection = false;
   
    [SerializeField]
    private float doorSpeed = 1.0f;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private bool MaxKinematicOnOpen = false;

    //Ineraction Features for Socket interactions, Simple Interactor
    [Header("Interaction Configurations")]
    [SerializeField]
    private XRSocketInteractor socketInteractor;

    [SerializeField]
    private XRSimpleInteractable simpleInteractor;

    private void Start()
    {
        socketInteractor?.selectEntered.AddListener((s) =>
        {
            OpenDoor();
            PlayOnStart();
        });

        socketInteractor?.selectExited.AddListener((s) =>
        {
            PlayOnEnd();
            //When we are done exiting, we don't want to reuse the socket
            socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true;
        });
        simpleInteractor?.selectEntered.AddListener((s) =>
        {
            OpenDoor();

        });       
    
    }

    public void OpenDoor()
    {
        //if(doorOpen ? false : true)
      //  {
        //    PlayOnStart();
        //}
        if(!open)
        {
            PlayOnStart();
            open = true;
            StartCoroutine(ProcessMotion());        
        }
    }
    private IEnumerator ProcessMotion()
    {
        var angle = doorPivot.localEulerAngles.y < 180 ? doorPivot.localEulerAngles.y : doorPivot.localEulerAngles.y - 360;
        angle = reverseAngleDirection ? Mathf.Abs(angle) : angle;
        if (angle<= maxAngle)
        {
            doorPivot?.Rotate(Vector3.up,doorSpeed * Time.deltaTime * (reverseAngleDirection ? -1 : 1));
   
        }
        else
        {
            open = false;
            var featureRigidBody  = GetComponent<Rigidbody>();
            if (featureRigidBody != null && MaxKinematicOnOpen) featureRigidBody.isKinematic = true;
            
        }
    
        yield return (null);
    
    
    }



}
