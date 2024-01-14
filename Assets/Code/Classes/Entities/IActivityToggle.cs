/*
 * Offers methods that enable or disable the activity of an entity.
 * This usually means that the entity will not move or do any other action.
 */
public interface IActivityToggle
{
    public void DisableActivity();
    public void EnableActivity();
}
