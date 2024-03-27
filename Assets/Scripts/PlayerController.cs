using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] private FloatingJoystick joystick;
    // Status
    private Rigidbody playerRb;
    [SerializeField] private float speed = 5.0f; 
    [SerializeField] private float playerWeight = 5.0f;
    private GameObject focalPoint;
    [SerializeField] private bool hasPowerup;
    [SerializeField] private bool hasSpeedup;
    [SerializeField] private bool hasDisArm;
    // [SerializeField] private bool isALive;
    private float powerupStrength = 15.0f;
    [SerializeField] private GameObject pickUpItemPrefab; 
    //Ground Barrier
    private Rigidbody groundBarrierRB;
    //Buff and debuff
    [SerializeField] private GameObject powerupIndicator, speedupIndicator, speedupParticle, powerupParticle, powerupWind;
    [SerializeField] private GameObject snow, disArm, slowDown, blastWaveTrigger; 
    // Charge
    // [SerializeField] private float initialJumpForce = 10f;
    // [SerializeField] private float maxJumpForce = 30f;
    // [SerializeField] private float jumpForceIncrement = 10f;

    // private bool isJumping = false;
    // private float currentJumpForce = 0f;
    //Ground barrier
    private GameObject groundBarrier;
    private Vector3 gBStartPos, gBEndPos, playerScale;

    void Start()
    {   
        playerRb =  GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        groundBarrier = GameObject.FindWithTag("GroundBarrier");
        groundBarrierRB = groundBarrier.GetComponent<Rigidbody>();
        DisableRagdoll();

        gBStartPos = groundBarrier.transform.localPosition;
        gBEndPos = gBStartPos + new Vector3(0f, 0f, 2.2f);
        // isALive = true;

    }

    // Update is called once per frame
    void Update()
    {   
        // Control
        playerRb.AddForce(focalPoint.transform.forward * speed * joystick.Vertical * playerWeight);
        playerRb.AddForce( 1.5f * Vector3.right * speed * joystick.Horizontal * playerWeight);

        // Interacting
        powerupIndicator.transform.position = transform.position + new Vector3(0f, 5f, 0f);
        powerupWind.transform.position = transform.position + new Vector3(0f, -2f, 0f);
        speedupIndicator.transform.position = transform.position + new Vector3(0f, 6f, 0f);
        blastWaveTrigger.transform.position = transform.position + new Vector3(0, 0, 0);
        
        snow.transform.position = transform.position;
        disArm.transform.position = transform.position;
        slowDown.transform.position = transform.position;

    }
    // FixedUpdate
    void FixedUpdate()
    {
        // if (isJumping)
        // {
        //     // Gradually increase the jump force while holding the screen
        //     currentJumpForce = Mathf.Clamp(currentJumpForce + jumpForceIncrement * Time.fixedDeltaTime, initialJumpForce, maxJumpForce);
        //     playerRb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
        // }
    }
    //  Buff
    private void OnTriggerEnter(Collider other){
            
        //PowerUp
        if (other.CompareTag("Powerup") && hasDisArm == false){
            hasPowerup = true;
            other.gameObject.SetActive(false);

            StartCoroutine(PowerupCountdownRoutine());
            StartCoroutine(PowerupWindCountdownRoutine());
            StartCoroutine(BlastWaveTriggerCountdownRoutine());
            
            
            powerupIndicator.gameObject.SetActive(true);
            powerupWind.gameObject.SetActive(true);
            powerupParticle.gameObject.SetActive(true);
            blastWaveTrigger.gameObject.SetActive(true);
        } 
        //SpeedUp
        if(other.CompareTag("Speedup")){
            Instantiate(pickUpItemPrefab, GenerateAbsorbPosition(), pickUpItemPrefab.transform.rotation);

            hasSpeedup= true;
            speed = speed * 1.5f;
            other.gameObject.SetActive(false);
            StartCoroutine(SpeedupCountdownRoutine());
            
            speedupIndicator.gameObject.SetActive(true);
            speedupParticle.gameObject.SetActive(true);

        }
        
        
        if (other.CompareTag("Powerup") && hasDisArm == true)
        {
            hasPowerup = false;
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Barrier")){
            // isALive = false;
        }

    }
    //Lightning
    private Vector3 GenerateAbsorbPosition(){
        float spawnPosX = transform.position.x;
        float spawnPosZ = transform.position.z;
        Vector3 spawnPos = new Vector3(spawnPosX, 32, spawnPosZ);
        return spawnPos;
    }

    //Knockback
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("GroundBarrier") && hasPowerup){
            Rigidbody enemyRigidbody =  collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * 2 * powerupStrength, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("Button") ){
            collision.gameObject.SetActive(false);
    //Barrier Up
            EnableRagdoll();
            groundBarrier.transform.localPosition = Vector3.Lerp (gBStartPos, gBEndPos, 4f);
        }
        
        if (collision.gameObject.CompareTag("BlueEnemy") ){
                if (hasSpeedup == true){
                    collision.gameObject.SetActive(false);
                }
            Debug.Log("Frozen!");
            snow.gameObject.SetActive(true);         
            StartCoroutine(SnowCountdownRoutine()); 
            speed = speed - 5f;
        }

        if (collision.gameObject.CompareTag("OrangeEnemy") ){    
            Debug.Log("HasDisArm!");   
            hasPowerup = false;
            hasDisArm = true;
            StartCoroutine(DisArmCountdownRoutine());
            disArm.gameObject.SetActive(true);
            powerupIndicator.gameObject.SetActive(false);
            powerupParticle.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("GreenEnemy") ){
            Debug.Log("Slowed!");
            slowDown.gameObject.SetActive(true);         
            StartCoroutine(SlowDownCountdownRoutine()); 
            speed = speed - Time.deltaTime * 150f;
        }

    }

    //Ground Barrier envolved function
    void EnableRagdoll()
    {
        groundBarrierRB.isKinematic = false;
        groundBarrierRB.detectCollisions = true;
    }

    // Let animation control the rigidbody and ignore collisions.
    void DisableRagdoll()
    {
        groundBarrierRB.isKinematic = true;
        groundBarrierRB.detectCollisions = false;
    }

    //Buff Countdown
        IEnumerator PowerupCountdownRoutine(){
            yield return new WaitForSeconds(7);
            hasPowerup = false;
            powerupIndicator.gameObject.SetActive(false);
            powerupParticle.gameObject.SetActive(false);
            //Destroy(pickUpItemPrefab.gameObject);
            Debug.Log("Cooled down!");
        }

        IEnumerator BlastWaveTriggerCountdownRoutine(){
            yield return new WaitForSeconds(1);
            blastWaveTrigger.gameObject.SetActive(false);            
        }

         IEnumerator PowerupWindCountdownRoutine(){
            yield return new WaitForSeconds(3);
            powerupWind.gameObject.SetActive(false);
        }

        IEnumerator SpeedupCountdownRoutine(){
            yield return new WaitForSeconds(10);
            hasSpeedup = false;
            speedupIndicator.gameObject.SetActive(false);
            speedupParticle.gameObject.SetActive(false);
            pickUpItemPrefab.gameObject.SetActive(false);
            Debug.Log("Speed down!");
        }


        //Debuff countdown
        IEnumerator SnowCountdownRoutine(){
            yield return new WaitForSeconds(1);
            snow.gameObject.SetActive(false);
            speed = speed + 5f;
            Debug.Log("Defrost!");
        }

        IEnumerator DisArmCountdownRoutine(){
            yield return new WaitForSeconds(2.5f);
            hasDisArm =  false;
            disArm.gameObject.SetActive(false);
            Debug.Log("UnDisArm!"); 
        }

        IEnumerator SlowDownCountdownRoutine(){
            yield return new WaitForSeconds(3.5f);
            slowDown.gameObject.SetActive(false);
            speed = 5.0f;
            Debug.Log("UnSlowed!");
        }
        
    // void StartJump()
    // {
    //     // Initialize the jump
    //     isJumping = true;
    //     currentJumpForce = initialJumpForce;
    // }

    // void EndJump()
    // {
    //     // End the jump when you release your fingers
    //     isJumping = false;
    //     currentJumpForce = 0f;
    // }
    
}

