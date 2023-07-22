using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMotion : MonoBehaviour
{
    public AudioSource Instruction;
    public Vector3 AverVel = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }


     public static Vector3 FindAverageVelocity<TComponent>(TComponent[] components) where TComponent : Component
		{
			Vector3 TotalVel = Vector3.zero;
			foreach (TComponent component in components)
			{
				if (component == null)
				{
					continue;
				}
				var rigidbody = component.GetComponent<Rigidbody>();
				if (rigidbody == null)
				{
					continue;
				}
				TotalVel += rigidbody.velocity;
			}
            Vector3 AverVel = TotalVel / components.Length;
            return AverVel;
            Debug.Log(AverVel.magnitude);
		 }

    // Update is called once per frame
    void Update()
    {
          if (AverVel.magnitude > 10)
          {
              Debug.Log("Play Instructions:");
              //Instruction.Play;
          }
    }

}