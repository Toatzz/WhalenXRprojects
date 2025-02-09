using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerFeatures : CoreFeatures
{

    [Header("Drawer Configuration")]
    [SerializeField]
    private Transform drawerSlide;

    [SerializeField]
    private float maxDistance = 1.0f;

    [SerializeField]
    private FeatureDirection featureDirection = FeatureDirection.Forward;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private XRSimpleInteractable simpleInteractable;
    
    void Start()
    {
        //drawers with simple interactable
        simpleInteractable?.selectEntered.AddListener((s) =>
        {
            //if drawer is not open 
            if(!open)
            {
                OpenDrawer();
            }
        });
    }

    private void OpenDrawer()
    {
        open = true;
        PlayOnStart();
        StartCoroutine(ProcessMotion());
    }
    private IEnumerator ProcessMotion()
    {
        while (open)
        {
            if (featureDirection == FeatureDirection.Forward && drawerSlide.localPosition.z >= maxDistance)
            {
                drawerSlide.Translate(Vector3.forward * Time.deltaTime * speed);
            }
            else if (featureDirection == FeatureDirection.Backward && drawerSlide.localPosition.z <= maxDistance)
            {
                drawerSlide.Translate(-Vector3.forward * Time.deltaTime * speed);
            }

            else
            {
                open = false;   //Ends the loop
            }
            yield return null;

        }

    }

}
