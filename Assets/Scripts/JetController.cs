using UnityEngine;

public class JetController : MonoBehaviour
{
    //SEPERATE THE CAMERA ROTATION TO THE JET ROTATION - CAN USE CINEMACHINE
    public Rigidbody rb;

    //JOYSTICK REQUIRED
    //[SerializeField] Joystick joystick;

    public static JetController Instance { get; private set; }

    private float horizantalInput;
    private float verticalInput;

    //SPEED PARTICLE
    public ParticleSystem particle;

    //THIS IS RELATIVELY "SLOW JET" YOU CAN INCREASE THE SPEED VALUES
    //SPEEDS
    public float forwardSpeed = 80f;
    [SerializeField] private float forwardSpeedMultiplier = 100f;

    public float horizantalSpeed = 110f;
    public float verticalSpeed = 50f;
    [SerializeField] private float speedMultiplier = 100f;

    //ROTATIONS
    [SerializeField] private float maxHorizontalRotation = .6f;
    [SerializeField] private float maxVerticalRotation = .3f;

    //SMOOTHNESS
    [SerializeField] private float rotationSmoothness = .7f;
    [SerializeField] private float smoothness = .3f;

    public bool moveable = false;
    [SerializeField] private GameObject text;

    private void Awake()
    {
        //SINGLETON
        Instance = this;

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //MOVE INPUTS:
        if (moveable)
        {
            //TOUCH OR MOUSE LEFT TO SCREEN
            if (Input.GetMouseButton(0) || Input.touches.Length != 0)
            {
                //JOYSTICK REQUIRED
                //horizantalInput = joystick.Horizontal;

                //JOYSTICK REQUIRED
                //verticalInput = joystick.Vertical;
            }
            else
            {
                //WASD
                horizantalInput = Input.GetAxisRaw("Horizontal");
                verticalInput = Input.GetAxisRaw("Vertical");
            }
        }
        else
        {
            //WHILE NOT MOVING THERE IS NO SPEED PARTICLE
            particle.Stop();
        }

        // PRESS SPACE OR THE BUTTON TO START THE GAME 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTheGame();
        }

        HandlePlaneRotation();

    }
    //IF JET COLLIDE WITH OBSTACLES OR PLANE DIES
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            moveable = false;
            rb.useGravity = true;
            forwardSpeed = 0;
            horizantalSpeed = 0;
            verticalSpeed = 0;
            Debug.Log("YOU ARE DEAD");
        }
    }

    public void PlayTheGame()
    {
        moveable = true;
        particle.Play();
        text.SetActive(false);
    }

    private void FixedUpdate()
    {
        HandlePlaneMovement();
    }

    private void HandlePlaneRotation()
    {
        //FLOAT = PRESSED? * VALUE
        float horizontalRotation = horizantalInput * maxHorizontalRotation;

        //FLOAT = PRESSED? * VALUE
        float verticalRotation = verticalInput * maxVerticalRotation;
        
        //IF YOU WANT "W" TO BE UP AND "S" TO BE DOWN ADD THE X ROT "-".
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(verticalRotation, transform.rotation.y
            , -horizontalRotation, transform.rotation.w)
            , Time.deltaTime * rotationSmoothness);
    }
    private void HandlePlaneMovement()
    {
        if (moveable)
        {
            //MOVE
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed * forwardSpeedMultiplier * Time.deltaTime);
        }

        //VELOCITY VALUES
        float xVelocity = horizantalInput * speedMultiplier * horizantalSpeed * Time.deltaTime;
        float yVelocity = -verticalInput * speedMultiplier * verticalSpeed * Time.deltaTime;

        //INCREASING VELOCITY IN TIME
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(xVelocity, yVelocity, rb.velocity.z), Time.deltaTime * smoothness);
    }

}
