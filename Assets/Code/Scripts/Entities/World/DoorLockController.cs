using UnityEngine;

public class DoorLockController : LockController
{
    protected override void DisableLock()
    {
        base.DisableLock();

        transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
    }
}
