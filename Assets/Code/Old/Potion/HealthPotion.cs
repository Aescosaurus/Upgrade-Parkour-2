using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion
	:
	PotionBase
{
	protected override void Drink()
	{
		animCtrl.GetComponent<Damageable>().Heal( healAmount );
	}

	[SerializeField] float healAmount = 7.0f;
}
