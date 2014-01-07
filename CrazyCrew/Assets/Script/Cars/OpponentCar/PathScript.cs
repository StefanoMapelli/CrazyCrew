using UnityEngine;
using System.Collections;

public class PathScript : MonoBehaviour {

	private ArrayList path;
	private Color rayColor = Color.white;

	private void OnDrawGizmos()
	{
		Transform[] pathObjs=transform.GetComponentsInChildren<Transform>();
		path= new ArrayList();


		foreach (Transform pathObj in pathObjs)
		{
			if(pathObj!=transform)
				path.Add(pathObj);
		}

		for(int i=0; i<path.Count; i++)
		{
			Vector3 pos = ((Transform)path[i]).position;
			if(i>0)
			{
				Vector3 prev = ((Transform)path[i-1]).position;
				Gizmos.DrawLine(prev,pos);
				Gizmos.DrawWireSphere(pos, 0.4f);

			}

		}
	}


	
}
