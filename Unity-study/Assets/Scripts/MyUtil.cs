using UnityEngine;

public static partial class MyUtil
{
    public readonly static LayerMask maskDefault = LayerMask.GetMask("Default");
    public readonly static LayerMask maskPlayer = LayerMask.GetMask("Player");
    public readonly static LayerMask maskBullet = LayerMask.GetMask("Bullet");
    public readonly static LayerMask maskMonster = LayerMask.GetMask("Monster");

    public readonly static int layerDefault = LayerMask.NameToLayer("Default");
    public readonly static int layerPlayer = LayerMask.NameToLayer("Player");
    public readonly static int layerBullet = LayerMask.NameToLayer("Bullet");
    public readonly static int layerMonster = LayerMask.NameToLayer("Monster");

}
