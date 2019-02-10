using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaysManager : MonoBehaviour
{
	public static int dayNumber = 0;

	public static void StartDay()
	{
		//AkSoundEngine.SetState("ST_Time", "Day");
	}

	public static void FinishDay()
	{
		foreach (NPCTalk npc in NPCTalk.NPC)
		{
			npc.FinishDay();
		}
	}
}
