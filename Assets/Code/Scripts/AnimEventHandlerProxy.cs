using UnityEngine;

/**
 * Proxy class used in order to forward any given method triggered by an AnimationEvent to a specific script.
 * This allows for more flexibility when implementing e.g. a Weapon, since AnimationEvents can trigger any
 * custom method at the respective target script, without the top level script to which the Animator has
 * access to is aware of them.
 * 
 * The only limitation is that since an AnimationEvent only send one parameter, which usually is the method
 * name, one can not forward a method call with other parameters. Yet, the implementation of the different
 * parameters could be customized in order to allow for more parameters, e.g. a string could include method
 * name and parameters with separating delimiter characters, which could then be split.
 */
public abstract class AnimEventHandlerProxy<T> : MonoBehaviour where T : Component
{
    protected void ForwardAnimEvent(T forwardTo, string functionName)
    {
        forwardTo.gameObject.SendMessage(functionName, SendMessageOptions.RequireReceiver);
    }
}

