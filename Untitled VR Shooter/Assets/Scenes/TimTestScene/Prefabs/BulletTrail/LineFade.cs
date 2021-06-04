using UnityEngine;
using System.Collections;

//This script is part of the package I downloaded from this tutorial:
//https://www.youtube.com/watch?v=AKgaXqSgGJk
//I made no modifications--I just wanted some sort of effect

namespace Bolt.AdvancedTutorial
{
	public class LineFade : MonoBehaviour
	{
		[SerializeField] private Color color;

        [SerializeField] private float speed = 10f;

		LineRenderer lr;

		void Start ()
		{
			lr = GetComponent<LineRenderer> ();
		}

		void Update ()
		{
			// move towards zero
			color.a = Mathf.Lerp (color.a, 0, Time.deltaTime * speed);

			// update color
			//lr.SetColors (color, color);
			lr.startColor = color;
			lr.endColor = color;
		}
	}
}
