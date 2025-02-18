using UnityEngine;

public class TerrainDetect : MonoBehaviour
{
    private Terrain GetCurrentTerrain(Vector3 PlayerPos)
    {
        Terrain[] terrains = Terrain.activeTerrains;

        foreach (Terrain terrain in terrains)
        {
            Vector3 terrainPos = terrain.transform.position;
            Vector3 terrainSize = terrain.terrainData.size;

            if(PlayerPos.x >= terrainPos.x && PlayerPos.x < terrainPos.x + terrainSize.x &&
                PlayerPos.z >= terrainPos.z && PlayerPos.z < terrainPos.z + terrainSize.z)
            {
                return terrain;
            }
        }
        return null;
    }

    protected virtual int GetTerrainAtPosition(Vector3 pos)
    {
        Terrain currentTerrain = GetCurrentTerrain(pos);
        if (currentTerrain == null) return -1;

        TerrainData terrainData = currentTerrain.terrainData;
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;
        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        int numTextures = splatmapData.GetLength(2);

        Vector3 TerrainCord = ConvertToSplatMapCoordinate(pos, currentTerrain);
        int ret = 0;
        float comp = 0f;
        for(int i = 0; i < numTextures; i++)
        {
            if (comp < splatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
                ret = i;
        }
        return ret;
    }

	private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos, Terrain terrain)
	{
		Vector3 vecRet = new Vector3();
		Vector3 terrainPos = terrain.transform.position;
		vecRet.x = ((playerPos.x - terrainPos.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
		vecRet.z = ((playerPos.z - terrainPos.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
		return vecRet;
	}
}
