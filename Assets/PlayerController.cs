using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool isMouseDown;
    private int shots = 0;
    private bool shotsLeft = true;
    public Vector3 currentPosition;
    public Transform center;
    public float maxLength;
    public float force;
    public int totalShots;
    public Rigidbody2D asteroid;
    public LineRenderer Trajectory;
    public Attractor playerAttraction;
    void Update(){
        if(isMouseDown){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);
            ShowTrajectory();
        }
    }
    void SetTrajectoryActive(bool active){
        Trajectory.enabled = active;
    }

    void ShowTrajectory(){
        SetTrajectoryActive(true);
        Vector3 diff = center.position - currentPosition;
        int segmentCount = 10;
        Vector2[] segments = new Vector2[segmentCount];
        segments[0] = asteroid.transform.position;

        Vector2 segVelocity = new Vector2(diff.x, diff.y) * 5;
        for(int i = 1; i < segmentCount; i++){
            float timeCurve = (i * Time.fixedDeltaTime * 5);
            segments[i] = segments[0] + segVelocity * timeCurve * 0.5f * Mathf.Pow(timeCurve,2);
        }
        Trajectory.positionCount = segmentCount;
        for(int j = 0; j < segmentCount; j++){
            Trajectory.SetPosition(j,segments[j]);
        }
    }

    private void OnMouseDown(){
        if(shots >= totalShots){
            shotsLeft = false;
        } else {
            shots++;
            isMouseDown = true; //Register that mouse went down
        }
    }

    private void OnMouseUp(){
        if(shotsLeft){
            isMouseDown = false;
            SetTrajectoryActive(false);
            Shoot();
        }
    }

    void Shoot(){
        if(shotsLeft){
            Vector3 asteroidForce = (currentPosition - center.position) * force * -1;
            asteroid.AddForce(asteroidForce);
            playerAttraction = GetComponent<Attractor>();
            playerAttraction.enabled = true;
        }
    }
}
