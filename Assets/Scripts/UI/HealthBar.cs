using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private void Start()
	{
		SetMaxHealth(45);
	}

	public Slider slider;
	public event Action OnDeerKilled ;
	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

	}

    public void SetHealth(int health)
	{
		slider.value -= health;
		if (slider.value <= 0)
		{
			OnDeerKilled?.Invoke();

			slider.value = 0;
		}
	}

}
