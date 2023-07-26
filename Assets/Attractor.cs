using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 6.674f;
    public Rigidbody2D rb;
    public static List<Attractor> toAttract;
    public Vector2 InitialVelocity;
    void Start(){
        rb.velocity = InitialVelocity;
    }
    void FixedUpdate(){
        foreach(Attractor attractor in toAttract){
            if(attractor != this){
                Attract(attractor);
            }
        }
    }

    void OnEnable(){
        if(toAttract == null){
            toAttract = new List<Attractor>();
        }

        toAttract.Add(this);
    }

    void onDisable(){
        toAttract.Remove(this);
    }
    void Attract(Attractor objToAttract){
        Rigidbody2D rbToAttract = objToAttract.rb;

        Vector2 direction = rb.position - rbToAttract.position;
        float distanceSqr = direction.sqrMagnitude;

        float forceMagnitude =G * (rb.mass * rbToAttract.mass) / distanceSqr;
        Vector2 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
