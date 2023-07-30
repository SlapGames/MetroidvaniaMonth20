using UnityEngine;

public abstract class Telekinesis1Susceptible: MonoBehaviour
{
    public abstract void HandleAffectedByTele1Windup(Telekinesis1Manager telekinesis1Manager);
    public abstract void HandleAffectedByTele1Action();
    public abstract void HandleAffectedByTele1Deactivate();
}
