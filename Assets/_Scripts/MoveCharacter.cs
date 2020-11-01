#if UNITY_IOS || UNITY_ANDROID
#define USING_MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveCharacter : MonoBehaviour

{
    public float playerSpeed = 10f;
    public float moveHorizontal;
    public float moveVertical;
    private Vector3 movement;
    private Rigidbody player;
    public float jumpForce = 10f;
    public bool _isGrounded;
    public float gravityMultiplier = 1.5f;
    public AudioClip walkSound, jumpSound;
    private AudioSource _audioSource;
    private bool _isWalking = false;
    private bool _isJumping = false;
    public Joystick joystick;
    private bool _isButtonJumping;
    private bool _isPlayerAtExit, _isGameEnd;
    public CanvasGroup endLevelCanvasGroup;
    public CanvasGroup failedLevelCanvasGroup;
    public float fadeDuration = 1f;

    public float timer;
    public GameObject timeTxt;
    private TimeClock _scriptTimeClock;

    Quaternion rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _scriptTimeClock = timeTxt.GetComponent<TimeClock>();
        Physics.gravity = gravityMultiplier * new Vector3(0, -9.81f, 0);
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isGameEnd)
        {
            playerSkills();

            if (_isPlayerAtExit || _scriptTimeClock.TiempoMaximoSuperado())
            {
                timer += Time.deltaTime;

                if (_isPlayerAtExit)
                {
                    _scriptTimeClock.Pausar();
                    _scriptTimeClock.CambiarAVerde();

                    endLevelCanvasGroup.blocksRaycasts = true;
                    endLevelCanvasGroup.interactable = true;
                    endLevelCanvasGroup.alpha = Mathf.Clamp(timer / fadeDuration, 0, 1);
                }
                else
                {
                    failedLevelCanvasGroup.blocksRaycasts = true;
                    failedLevelCanvasGroup.interactable = true;
                    failedLevelCanvasGroup.alpha = Mathf.Clamp(timer / fadeDuration, 0, 1);
                }

                if (timer > fadeDuration)
                {
                    EndLevel();
                }
            }
        }
    }

    void EndLevel()
    {
        _isPlayerAtExit = false;
        _isGameEnd = true;
        _audioSource.Stop();
        //Debug.Log("Termino el nivel");
    }

    void FixedUpdate()
    {
        if (!_isGameEnd)
        {
#if USING_MOBILE
            moveHorizontal = joystick.Horizontal;
            moveVertical = joystick.Vertical;


            /*moveHorizontal = Input.GetAxis("Mouse X");
            moveVertical = Input.GetAxis("Mouse Y");

            if (Input.touchCount > 0)
            {
                moveHorizontal = Input.touches[0].deltaPosition.x;
                moveVertical = Input.touches[0].deltaPosition.y;
            }*/
#else
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
#endif

            movement.Set(moveHorizontal, 0, moveVertical);
            movement.Normalize();

            bool isMovingHorizontalInput = !Mathf.Approximately(moveHorizontal, 0f);
            bool isMovingVerticalInput = !Mathf.Approximately(moveVertical, 0f);
            _isWalking = isMovingHorizontalInput || isMovingVerticalInput;

            //Dirección deseada donde el jugador va a mirar
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, playerSpeed * Time.fixedDeltaTime, 0f);

            rotation = Quaternion.LookRotation(desiredForward);


            //player.transform.LookAt(player.transform.position + _movement);


            //Movemos el personaje hacia los lados y hacia adelante
            // S = s0 + V*t*(dirección)
            player.MovePosition(player.position + (playerSpeed * Time.deltaTime * movement));
            player.MoveRotation(rotation);


            //Debug.Log(player.velocity.magnitude);
            //characterController.AddForce(new Vector3(0,1,0) * jumpForce * jump);
        }
    }

    //Función para controlar las habilidades de nuestro Jugador
    public void playerSkills()
    {
        //Saltar
        if ((Input.GetButtonDown("Jump") || _isButtonJumping) && _isGrounded)
        {
            //No se multiplica por Time.deltaTime ya que no se quiere una aceleración si no un impulso
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
            _audioSource.Stop();
            _audioSource.PlayOneShot(jumpSound, 0.8f);
            _isJumping = true;
            _isButtonJumping = false;
            Invoke("cambiarIsJumpingFalse", 1f);
        }

        if (!_isJumping)
        {
            if (_isWalking)
            {
                if (_isGrounded)
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.PlayOneShot(walkSound, 1);
                    }
                }
                else
                    _audioSource.Stop();
            }
            else
            {
                _audioSource.Stop();
            }
        }
    }

    private void cambiarIsJumpingFalse()
    {
        _isJumping = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            _isPlayerAtExit = true;
            //Debug.Log("Spawn");
        }
    }

    public void JumpButton()
    {
        if (!_isJumping)
            _isButtonJumping = true;
    }
}
