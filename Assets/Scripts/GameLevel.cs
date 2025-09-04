using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [field: SerializeField]
    public int LevelNumber { get; private set; }

    [field: SerializeField]
    public Transform PlayerSpawnPoint { get; private set; }
}
