using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGoal : MonoBehaviour
{
    public IGoal previousGoal;

    public abstract void StartGoal();
    public abstract void MissGoal();
    public abstract void FinishGoal();
}
