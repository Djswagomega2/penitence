using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
	public Sprite displayNote; 
	public enum MiscType
	{
		Note,
		Key
	}
	public MiscType miscType;

	public void showNote(imageCloseUp img) 
	{
		img.image.enabled = true;
		img.image.sprite = displayNote;
	}


	public override MiscClass GetMisc() { return this; }
}
