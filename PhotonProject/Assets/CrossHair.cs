using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public LayerMask layerMask;
    public SpriteRenderer dot;
    public Color EnemyCheckColor;
    public Color OriginalColor;
    public Vector3 cc;


    private void Start()
    {
      //  Cursor.visible = false;
      //  OriginalColor = dot.color;
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    
    }
    public void ScanTarget(Ray ray)
    {

        cc = ray.GetPoint(1);
        RaycastHit2D hit = Physics2D.Raycast(cc, transform.position, 10f,layerMask);
        if (hit)
        {
            dot.color = EnemyCheckColor;
        }
        else
        {
            dot.color = OriginalColor;
        }
      /*  if (Physics.Raycast(ray, 100, layerMask))
        {
           // Debug.Log(ray.GetPoint(1));


        }
        else
        {
            Debug.Log("22");

        }*/
    }
}
