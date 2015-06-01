using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestructor : MonoBehaviour {
	void OnEnable()	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()	{
		while(true)	{
			yield return new WaitForSeconds(0.5f);
			if(!this.GetComponent<ParticleSystem>().IsAlive(true)) {
				GameObject.Destroy(this.gameObject);
			}
		}
	}
}
