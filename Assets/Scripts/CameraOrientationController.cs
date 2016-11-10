using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraOrientationController : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		this.transform.LookAt( this.transform.position - new Vector3(1,1,1), Vector3.up);
	}
}
