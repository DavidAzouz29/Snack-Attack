using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ArrayLayout  {
	int iRow = 4;
	int iCol = 3;
	[System.Serializable]
	public struct arrayData{
		public Button[] row;
		public Button[] coloumn;
	}

	//arrayButton
	public arrayData[] playerRows = new arrayData[4]; //Grid of 7x7
	public arrayData[] playerColsButton = new arrayData[3]; //Grid of 7x7
}
