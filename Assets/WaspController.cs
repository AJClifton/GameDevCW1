using UnityEngine;

public class WaspController : MonoBehaviour
{

    public bool playerInSight = false;
    public bool charging = false;
    float chargeDelay = 1;
    public float timeLeftToCharge = 0;
    float range = 5;
    GameObject target;
    Vector3 restPosition;

    void Start()
    {
        restPosition = transform.position;
        
    }

    void Update()
    {
        if (playerInSight && !charging) {
            charging = true;
            timeLeftToCharge = chargeDelay;
        }
        if (charging && timeLeftToCharge > 0) {
            timeLeftToCharge -= Time.deltaTime;
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);
        }
        if (charging && timeLeftToCharge <= 0) {
            if (Vector3.Distance(restPosition, transform.position) < range + 2) {
                transform.position += transform.right * 8 * Time.deltaTime;
            }
            else {
                charging = false;
                playerInSight = false;
            }
        }
    }

    void CheckTargetInRange() {
        if (Vector3.Distance(transform.position, target.transform.position) < range) {
            playerInSight = true;
        }
        if (Vector3.Distance(transform.position, restPosition) > range && !charging) {
            playerInSight = false;
            transform.position = restPosition;
        }
    }

    public void SetTarget(GameObject t) {
        target = t;
        CancelInvoke();
        InvokeRepeating("CheckTargetInRange", UnityEngine.Random.value, 1f);
    }
}
