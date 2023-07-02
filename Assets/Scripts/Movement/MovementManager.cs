using UnityEngine;
namespace PirateGame.Movement{
    [RequireComponent(typeof(Sail))]
    [RequireComponent(typeof(Hull))]
    [RequireComponent(typeof(Crew))]
    public class MovementManager : MonoBehaviour
    {
            [SerializeField]
            Sail _sail;
            [SerializeField]
            Hull _hull;
            [SerializeField]
            Crew _crew;

            public void Setup(MoverDataStruct movementData){
                _sail.SetupMovementComponenet(_crew,_hull, movementData);
                _crew.SetupMovementComponenet(_sail, movementData);
            }
    }
}
