using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyGunshipAI : MonoBehaviour
{
    Ship ship;

    public float randomMoveFactor = .4f;
    public float randomFiringProbability = .2f;

    public Vector3 lastDestOffset;

    public float rangeToFire = 60;

    // Use this for initialization
    void Start()
    {
        ship = GetComponent<Ship>(); 
        ship.onEndTurnEvent = DoAIStuff;
    }

    public void DoAIStuff()
    {
        if (ship.maneuverSelected.targetSelected == null)
        {
            // then we are gonna select a damn target!
            var playerShips = GameManager
                .Instance
                .allShips.Where(p => p.isPlayer).ToArray();

            var playerIndex = Random.Range(0, playerShips.Length);
            var target = playerShips[playerIndex];
            ship.firingSolutiion.targetFiring = target;
            ship.maneuverSelected.targetSelected = target;

            var shouldIMove = Random.Range(0f, 1f) < randomMoveFactor;
            if(shouldIMove)
            {
                var moveRange = ship.maxMovementDistance;
                lastDestOffset = Random.insideUnitCircle * Random.Range(moveRange * (.25f), moveRange * (.8f));
            }

            //calculate 

            var destination = lastDestOffset + transform.position;

            // calculate orientation
            var orientation = Quaternion.LookRotation((target.transform.position -
            destination).normalized);

            ship.maneuverSelected.destinationLocalOffset = lastDestOffset;
            ship.maneuverSelected.targetOrientation = orientation;

            ship.ConfirmMoveSimple();

            // select firing solution
            var firingSol = ship.firingSolutiion;
            var inRange = Vector3.Distance(ship.transform.position, target.transform.position) <= rangeToFire;

            for(int i = 0; i <  10; i++){
                firingSol.fireCommand[i].Clear();
                var shouldIFireThisRound = Random.Range(0f, 1f) < randomFiringProbability;

                if (shouldIFireThisRound && inRange && ship.CanFireShots(0))
                {
                    ship.weaponShots++;
                    firingSol.fireCommand[i].Add(new FireCommand(ship.weapons[0]));
                }
            }

            //UpdateOrientationPosition(orientation);

            //if (shipSelected.firingSolutiion.targetFiring != null)
            //{
            //    attackLine.enabled = true;
            //}
            //ship.maneuverSelected.

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
