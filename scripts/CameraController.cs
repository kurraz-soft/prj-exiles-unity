using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public float Speed = 40.0f;

    void Update()
    {
        var v = Input.GetAxis("Vertical") * Time.unscaledDeltaTime * Speed;
        var h = Input.GetAxis("Horizontal") * Time.unscaledDeltaTime * Speed;

        this.transform.Translate(new Vector3(h,0,v), Space.World);
    }
}
