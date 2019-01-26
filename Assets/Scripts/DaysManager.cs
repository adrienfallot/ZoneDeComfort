using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaysManager : MonoBehaviour
{
	[System.Serializable]
	public struct Day
	{
        public IGoal[] goals;
	}

    public Day[] days;


	public void StartDay(int day)
	{
		IGoal[] dayGoals = days[day].goals;

		foreach (IGoal goal in dayGoals)
		{
			goal.StartGoal();
		}
	}

	public void FinishDay(int day)
	{
		IGoal[] dayGoals = days[day].goals;

		foreach (IGoal goal in dayGoals)
		{
			goal.MissGoal();
		}
	}
}
