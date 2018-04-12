using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deadEngine;
    [SerializeField] AudioClip riseEngine;

    [SerializeField] ParticleSystem thrustP;
    [SerializeField] ParticleSystem winP;
    [SerializeField] ParticleSystem deathP;

    bool transition = false;
    bool onDebug;

    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        onDebug = false;
    }
	
	void Update ()
    {
        if (!transition)
        {
            RotateInput();
            ThrustInput();
        }
        if (Debug.isDebugBuild)
        {
            DebugLoadNextLevel();
            DebugStopCollision();
        }
    }

    private void DebugLoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L)) //Debug Key L
        {
            LoadNextScene();
        }
    }

    private void DebugStopCollision()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (onDebug == false)
            {
                onDebug = true;
            }
            else if (onDebug == true)
            { onDebug = false; }
        }
    }

    private void ThrustInput()
    {    
        if (Input.GetKey(KeyCode.Space)) { ApplyThrust();}
        else  { StopThrust();}
    }

    private void StopThrust ()
    {
        audioSource.Stop();
        thrustP.Stop();
    }

    private void ApplyThrust()
    {  
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);            
        }
        thrustP.Play();
    }

    private void RotateInput()
    {
        rigidBody.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
                  
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
          
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transition == true) {return;}
        if (onDebug == true) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Hi");
                break;
            case "Finish":
                RiseSwitch();
                break;
            default:
                DeathSwitch();
                break;
        }
    } //to do: hit landing pad with nose

    private void DeathSwitch()
    {
        transition = true;
        audioSource.Stop();
        Invoke("DeadLoad",(levelLoadDelay + 0.5f));
        audioSource.PlayOneShot(deadEngine);
        deathP.Play();
    } //to do: Unfreeze positions when on death

    private void RiseSwitch()
    {
        transition = true;
        audioSource.Stop();
        Invoke("LoadNextScene", levelLoadDelay);
        audioSource.PlayOneShot(riseEngine);
        winP.Play();
    }

    private void DeadLoad()
    {SceneManager.LoadScene(0); }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1);
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        
    }
}
