using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gradient : MonoBehaviour
{
    public Material gradientMAT;
    public Color leftCol;
    public Color rightCol;
	private void Start()
	{
		gradientMAT.SetColor("_Color", leftCol);
		gradientMAT.SetColor("_Color2", rightCol);
	}
}
