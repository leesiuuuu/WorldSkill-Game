using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadRanking : MonoBehaviour
{
	public int _rank;
	public Text Rank;
	public Text Name;
	public Text Score;
	private void OnEnable()
	{
		GameSystem.instance.UpdateRanking();
		RankLoaded(_rank);
	}
	public void RankLoaded(int rank)
	{
		GameSystem.RankingList rankingList = new GameSystem.RankingList(); 

		string json = Path.Combine(Application.dataPath, "ranking.json");
		string jsondata = File.ReadAllText(json);

		rankingList = JsonUtility.FromJson<GameSystem.RankingList>(jsondata);

		if (rankingList.ranking.Count() < rank)
		{
			Debug.Log("데이터가 없음");

			Rank.text = rank.ToString();

			Name.text = "";
			Score.text = "";
			return;
		}

		Rank.text = rank.ToString();
		
		Name.text = rankingList.ranking[rank-1].name;
		Score.text = rankingList.ranking[rank - 1].score.ToString("000000");
	}
}
