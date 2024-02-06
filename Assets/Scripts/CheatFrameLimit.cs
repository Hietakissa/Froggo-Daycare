using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatFrameLimit : MonoBehaviour
{
	[SerializeField] [Range(1, 144)] int fpsTarget = 30;
	[SerializeField] bool setFpsCap = false;
	[SerializeField] bool disableFpsCap = false;
	void Update()
	{
		if (setFpsCap)
		{
			Application.targetFrameRate = fpsTarget;
			setFpsCap = false;
		}
		if (disableFpsCap)
		{
			Application.targetFrameRate = -1;
			disableFpsCap = false;
		}
	}


}
