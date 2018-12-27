using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ClickedPos = Input.mousePosition;
            Ray kRay = Camera.main.ScreenPointToRay(ClickedPos);
            RaycastHit rayCastHit;
            if (Physics.Raycast(kRay, out rayCastHit))
            {
                if ("Circle" == rayCastHit.collider.tag)
                {
                    GameMode gameMode = Camera.main.GetComponent<GameMode>();
                    gameMode.ClickedCircle();
                    ShowCircle(false);
                }
            }
        }
    }

    public void ShowCircle(bool bShow)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = bShow;
        SphereCollider sc = gameObject.GetComponent<SphereCollider>();
        sc.enabled = bShow;

        if(bShow)
        {
            Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-4.0f, 4.0f), 0.0f);
            gameObject.transform.SetPositionAndRotation(pos, new Quaternion());
        }
    }
}
