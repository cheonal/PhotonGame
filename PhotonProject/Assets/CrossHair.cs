using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public LayerMask layerMask;
    public SpriteRenderer dot;
    public Color EnemyCheckColor;
    Color OriginalColor;

    private void Start()
    {
      //  Cursor.visible = false;
        OriginalColor = dot.color;
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
    }
    public void ScanTarget(Ray ray)
    {
        if(Physics.Raycast(ray, 100, layerMask))
        {
            dot.color = EnemyCheckColor;
        }
        else
        {
            dot.color = OriginalColor;
        }
    }
}
