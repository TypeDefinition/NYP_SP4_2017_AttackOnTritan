using UnityEngine;
using System.Collections;

public class AIFollowPath_Boss : AIFollowPath {
	
	//Is there a wall obstructing us?
	public GameObject GetWall() {
		if (currentGrid == null) {
			return null;
		}
		return currentGrid.GetComponent<Grid>().wall;
	}

}