using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerManager : MonoBehaviour
{

    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    [Range(0f,1f)] public float maxSpeed;
    [Range(0f, 1f)] public float camSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    [Range(0f, 1000f)] public float ballRotateSpeed;
    private float velocity,camvelocity_x,camvelocity_y;
    private Camera mainCamera;
    public Transform path;
    public Rigidbody rb;
    public Collider collider;
    public Renderer ballRenderer;
    public ParticleSystem CollideParticle;
    public ParticleSystem airEffects;
    public ParticleSystem Dust;
    public ParticleSystem Balltrail;
    public Material[] Ball_material = new Material[2];

    // Start is called before the first frame update
    void Start()
    {

        rb= GetComponent<Rigidbody>();
        ball = transform;
        mainCamera = Camera.main;  
        collider= GetComponent<Collider>();
        ballRenderer= ball.GetChild(1).GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManager.menuManagerinstance.gameState)
        {
            moveTheBall = true;
            Balltrail.Play();
            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray, out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }
            if (moveTheBall)
            {

                Plane newPlan = new Plane(Vector3.up, 0f);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (newPlan.Raycast(ray, out var distance))
                {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 MouseNewPos = mouseNewPos - startBallPos;
                Vector3 DesireBallPos = MouseNewPos + startBallPos;

                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -1.5f, 1.5f);

                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed), ball.position.y, ball.position.z);
                
            }
        }
        if (MenuManager.menuManagerinstance.gameState)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));
            ball.GetChild(1).Rotate(Vector3.right * ballRotateSpeed * Time.deltaTime);
        }
      


    }

    private void LateUpdate()
    {
        var CameraNewPos = mainCamera.transform.position;
        if (rb.isKinematic)
            mainCamera.transform.position = new Vector3(Mathf.SmoothDamp(CameraNewPos.x, ball.transform.position.x, ref camvelocity_x, camSpeed), Mathf.SmoothDamp(CameraNewPos.y, ball.transform.position.y + 3f, ref camvelocity_y, camSpeed), CameraNewPos.z);
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            gameObject.SetActive(false);
            MenuManager.menuManagerinstance.gameState = true;
            MenuManager.menuManagerinstance.GameOver.SetActive(true);
        }

        switch (other.tag)
        {
            case "red":

                other.gameObject.SetActive(false);
                Ball_material[1]= other.GetComponent<Renderer>().material;
                ballRenderer.material = Ball_material[1];
                var NewParticle = Instantiate(CollideParticle, transform.position, Quaternion.identity);
                NewParticle.GetComponent<Renderer>().material=other.GetComponent<Renderer>().material;
                var BallTrail = Balltrail.trails;
                BallTrail.colorOverLifetime= other.GetComponent<Renderer>().material.color;
                break;

            case "violet":

                other.gameObject.SetActive(false);
                Ball_material[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = Ball_material[1];
                var NewParticle1 = Instantiate(CollideParticle, transform.position, Quaternion.identity);
                NewParticle1.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail1 = Balltrail.trails;
                BallTrail1.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

            case "green":

                other.gameObject.SetActive(false);
                Ball_material[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = Ball_material[1];
                var NewParticle2 = Instantiate(CollideParticle, transform.position, Quaternion.identity);
                NewParticle2.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail2 = Balltrail.trails;
                BallTrail2.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

            case "yellow":

                other.gameObject.SetActive(false);
                Ball_material[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = Ball_material[1];
                var NewParticle3 = Instantiate(CollideParticle, transform.position, Quaternion.identity);
                NewParticle3.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail3 = this.Balltrail.trails;
                BallTrail3.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

                default:
                break;
        }
        
        if (other.CompareTag("Finisher"))
        {
            MenuManager.menuManagerinstance.gameState = false;
            MenuManager.menuManagerinstance.Finish.SetActive(true);
        }
        if (other.gameObject.name.Contains("Colorball"))
        {
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 10);
            MenuManager.menuManagerinstance.menuElement.GetComponent<Text>().text.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("path"))
        {
            rb.isKinematic = collider.isTrigger = false;
            rb.velocity = new Vector3(0f, 8f, 0f);
            pathSpeed = pathSpeed * 2;
             var aireffectMain=airEffects.main;

            aireffectMain.simulationSpeed = 10f;

            Balltrail.Stop();
            ballRotateSpeed = 1000f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("path"))
        {
            rb.isKinematic= collider.isTrigger = true;
            pathSpeed = 32f;
            var aireffectMain = airEffects.main;

            aireffectMain.simulationSpeed = 4f;

            Dust.transform.position = collision.contacts[0].point+new Vector3(0f,0.3f,0f);
            Dust.GetComponent<Renderer>().material=ballRenderer.material;
            Dust.Play();

            Balltrail.Play();
            ballRotateSpeed = 500f;
        }
    }

}
