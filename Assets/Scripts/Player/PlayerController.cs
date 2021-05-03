using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Harpoon gun
    [Header("Harpoon Gun")]
    public GameObject gunHolder;
    public GameObject harpoonToFire;
    public float harpoonSpeed;
    public float fireDelay;
    private bool hasHarpoonReady = false;

    //Water movement
    [Header("Water Movement")]
    public float waterDrag = 4.0f;
    public float waterSpeed = 6.0f;
    public float landSpeed = 10.0f;
    public float depthChangeSpeed = 100f;


    private float currentSpeed = 10.0f;
    private float oceanLevel = 0.0f;

    private Rigidbody playerRB;
    private MouseCameraController playerCam;
    private bool isUnderwater = false;

    [Header("Key")]
    public GameObject key;
    public bool hasKey = false;

    [Header("Treasure")]
    public bool hasTreasure = false;

    [Header("Misc")]
    public PauseMenu pause;
    public MessageDisplay messageDisplay;

    void Start()
    {
        oceanLevel = GameObject.Find("OceanPlane").transform.position.y;
        playerRB = GetComponent<Rigidbody>();
        playerCam = transform.Find("Main Camera").GetComponent<MouseCameraController>();
        setHarpoonStatus(true);
    }

    
    IEnumerator FunctionName(){
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("VictoryScreen");
    }

    void Update()
    {

        if (hasTreasure)
        {
            StartCoroutine(FunctionName());
        }
        if(key.activeSelf){
            hasKey = true;
        }

        if (Input.GetButtonDown ("Fire1") && !pause.isGamePaused() && !messageDisplay.hasMessageOpen() && hasHarpoon())
        {
            FireHarpoon();
            setHarpoonStatus(false);
        }

        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");
            
        Vector3 move = new Vector3(x, 0.0f, z);
        playerRB.AddRelativeForce(move * currentSpeed * 50 * Time.deltaTime);
        if(isUnderwater)
        {
            if(Input.GetButton("Ascend"))
            {
                playerRB.AddForce(new Vector3(0, depthChangeSpeed * currentSpeed * Time.deltaTime, 0));
            }
            else if(Input.GetButton("Descend"))
            {
                playerRB.AddForce(new Vector3(0, -depthChangeSpeed * currentSpeed * Time.deltaTime, 0));
            }
        }
      
        CheckOceanPosition();
    }
    
    public bool hasHarpoon()
    {
        return hasHarpoonReady;
    }

    public void setHarpoonStatus(bool c)
    {
        hasHarpoonReady = c;
        gunHolder.SetActive(c);
    }

    public bool isInWater()
    {
        return isUnderwater;
    }

    private void CheckOceanPosition()
    {
        // The player is under the water
        if (transform.position.y < oceanLevel)
        {
            isUnderwater = true;
            playerRB.drag = waterDrag;
            currentSpeed = waterSpeed;
        }
        // The player is above the water
        else
        {
            isUnderwater = false;
            playerRB.drag = 1.0f;
            currentSpeed = landSpeed;
        }
    }

    private void FireHarpoon ()
    {
        GameObject harpoonObj = Instantiate(harpoonToFire, gunHolder.transform.position, gunHolder.transform.rotation)as GameObject;
        harpoonObj.GetComponent<Rigidbody>().velocity = (harpoonObj.transform.forward * harpoonSpeed);
        harpoonObj.GetComponent<TrailRenderer>().emitting = true;
    }
}
