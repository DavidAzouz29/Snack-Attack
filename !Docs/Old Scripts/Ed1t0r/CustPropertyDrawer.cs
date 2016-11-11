using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {

	int rows = 4;//(int)PlayerManager.MAX_PLAYERS;
	int coloumns = 3;
	SerializedProperty playerButtons;
	//arrayData
	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 18f;
		//data.rows[0][]
		for (int j = 0; j < rows; j++) {			
			playerButtons = property.FindPropertyRelative ("playerRows");
			SerializedProperty row = playerButtons.GetArrayElementAtIndex (j).FindPropertyRelative ("row");

			if (row.arraySize != rows) {
				row.arraySize = rows;
			}
			for (int k = 0; k < coloumns; k++) {
				playerButtons = property.FindPropertyRelative ("playerColsButton");
				SerializedProperty coloumn = playerButtons.GetArrayElementAtIndex (k).FindPropertyRelative ("coloumn");
				newposition.height = 18f;

				if (coloumn.arraySize != coloumns) {
					coloumn.arraySize = coloumns;
				}

			}
			newposition.width = position.width / coloumns;
			for (int i = 0; i < coloumns; i++) {
				EditorGUI.PropertyField (newposition, row.GetArrayElementAtIndex (i), GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 18f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 18f * (rows + 1);//8;
	}
}
