using UnityEngine;

public class Meteor : Obstacle
{
    protected override void Despawn()
    {
        Camera.main.GetComponent<CameraShake>().Shake();
        base.Despawn();
    }
}