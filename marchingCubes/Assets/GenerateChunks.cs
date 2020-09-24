using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChunks : MonoBehaviour
{
    IDictionary<Vector3, bool[,,]> maps = new Dictionary<Vector3, bool[,,]>();
    IDictionary<Vector3, GameObject> chunks = new Dictionary<Vector3, GameObject>();
	List<Vector3> keys = new List<Vector3>();
    public GameObject[,,] cubes;

	public GameObject chunkPrefab;
	Vector3 roundedPlayerPos;
    public Transform player;
    public int chunkViewRange;

    private void Update()
    {
        Vector3 pos = player.position;
		roundedPlayerPos = new Vector3(Mathf.Round(pos.x / 16) * 16, Mathf.Round(pos.y / 16) * 16, Mathf.Round(pos.z / 16) * 16);
        //Debug.Log(pos);
		manageChunks();
    }
    
	void manageChunks()
	{
		for (int x = -chunkViewRange/2; x < chunkViewRange/2; x++)
		{
			for (int y = -chunkViewRange/2; y < chunkViewRange/2; y++)
			{
				for (int z = -chunkViewRange/2; z < chunkViewRange/2; z++)
				{
					Vector3 pos = new Vector3(x*16 + roundedPlayerPos.x, y* 16 + roundedPlayerPos.y, z* 16 + roundedPlayerPos.z);
					//Vector3 pos = new Vector3(x*16 + roundedPlayerPos.x, y*16 , z*16);
					if(!chunks.ContainsKey(pos) || chunks[pos] == null)
					{
						//Debug.Log(pos);
						chunks.Add(pos,Instantiate(chunkPrefab, pos, transform.rotation,transform));
						keys.Add(pos);
					}
				}
			}
		}
		for(int i = 0; i < keys.ToArray().Length; i++)
		{
			Vector3 test = keys[i];
			if (Vector3.Distance(test, player.position) > chunkViewRange * 16)
			{
				Destroy(chunks[test]);
				chunks.Remove(test); keys.RemoveAt(i);
			}
			
		}
	}
}
