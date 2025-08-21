using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour
{
    [Tooltip("The event channel to listen to")]
    [SerializeField]
    private GameEventChannelSO<T> _event;

    [Tooltip("The response to invoke when the event is raised")]
    [SerializeField]
    private UnityEvent<T> response;

    private void OnEnable()
    {
        if (_event != null)
        {

            _event.OnEventRaised += Respond;
        }
    }

    private void OnDisable()
    {
        if (_event != null)
        {
            _event.OnEventRaised -= Respond;
        }
    }

    protected virtual void Respond(T value)
    {
        response?.Invoke(value);
    }
}
