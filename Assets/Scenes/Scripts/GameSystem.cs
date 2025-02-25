using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSystem
{
    public static GameSystem instance { get; } = new GameSystem();
	private GameSystem() { }

	public float resultTime = 0f;
	public int Money;

	public int Score;

	public enum WheelType
	{
		Normal,
		Sand,
		Mountain,
		Road
	};

	public enum Engine
	{
		Normal,
		_6Engine,
		_8Engine
	}

	public enum Transmission
	{
		Normal,
		EnforcedTransmission,
		AutoTransmission
	};

	//현재 상태를 저장함
	public WheelType wheeltype;
	public Engine engine;
	public Transmission transmission;

	public bool[] WheelStore = new bool[4];
	public bool[] EngineStore = new bool[3];
	public bool[] TransmissionStore = new bool[3];

	private class SaveData
	{
		public string wheeltype;
		public string engine;
		public string transmission;

		public int money;
	}

	private class StoreData
	{
		public bool[] WheelStore = new bool[4];
		public bool[] EngineStore = new bool[3];
		public bool[] TransmissionStore = new bool[3];
	}

	[System.Serializable]
	public class Ranking
	{
		public string name;
		public int score;
	}
	[System.Serializable]
	public class RankingList	
	{
		public List<Ranking> ranking = new List<Ranking>();
	}

	private List<Ranking> ranks = new List<Ranking>();
	public void Init()
	{
		WheelStore[0] = true;
		EngineStore[0] = true;
		TransmissionStore[0] = true;
		engine = Engine.Normal;
		transmission = Transmission.Normal;
		wheeltype = WheelType.Normal;
		SaveItemData();
	}
	public void SaveItemData()
	{
		SaveData data = new SaveData
		{
			wheeltype = this.wheeltype.ToString(),
			engine = this.engine.ToString(),
			transmission = this.transmission.ToString(),
			money = Money
		};

		string json = JsonUtility.ToJson(data, true);
		string path = Path.Combine(Application.dataPath, "playerData.json");

		File.WriteAllText(path, json);
	}
	public void LoadItemData()
	{
		SaveData data = new SaveData { };

		string path = Path.Combine(Application.dataPath, "playerData.json");
		string jsonData = File.ReadAllText(path);

		data = JsonUtility.FromJson<SaveData>(jsonData);

		wheeltype = (WheelType)Enum.Parse(typeof(WheelType), data.wheeltype);
		engine = (Engine)Enum.Parse(typeof(Engine), data.engine);
		transmission = (Transmission)Enum.Parse(typeof(Transmission), data.transmission);
		Money = data.money;
	}

	public void SaveStoreData()
	{
		StoreData data = new StoreData
		{
			WheelStore = this.WheelStore,
			EngineStore = this.EngineStore,
			TransmissionStore = this.TransmissionStore
		};

		string json = JsonUtility.ToJson (data, true);
		string path = Path.Combine(Application.dataPath, "storeData.json");

		File.WriteAllText(path, json);
	}
	public void LoadStoreData()
	{
		StoreData data = new StoreData { };

		string path = Path.Combine(Application.dataPath, "storeData.json");
		if (!File.Exists(path))
		{
			SaveStoreData();
		}
		string jsonData = File.ReadAllText (path);

		data = JsonUtility.FromJson<StoreData>(jsonData);

		WheelStore = data.WheelStore;
		EngineStore = data.EngineStore;
		TransmissionStore = data.TransmissionStore;
	}
	
	public void SetStoreData(Vector2 Itemindex, bool value)
	{
		switch(Itemindex.x)
		{
			case 0:
				WheelStore[(int)Itemindex.y] = value; break;
			case 1:
				EngineStore[(int)Itemindex.y] = value; break;
			case 2:
				TransmissionStore[(int)Itemindex.y] = value; break;
		}
	}
	public bool GetStoreData(Vector2 Itemindex)
	{
		switch (Itemindex.x)
		{
			case 0:
				return WheelStore[(int)Itemindex.y];
			case 1:
				return EngineStore[(int)Itemindex.y];
			case 2:
				return TransmissionStore[(int)Itemindex.y];
		}
		return false;
	}
	public void RegisterRanking(string name, int score)
	{
		Ranking rank = new Ranking
		{
			name = name,
			score = score
		};
		ranks.Add(rank);
		ranks.Sort((a, b) => b.score.CompareTo(a.score));

		UpdateRanking();
	}
	//랭킹 리스트를 업데이트하여 json에 저장함. init 대신 사용할 수 있음.
	public void UpdateRanking()
	{
		Debug.Log("UpdateRanking 실행됨");

		// 리스트 크기 확인
		Debug.Log($"현재 RankingList 크기: {ranks.Count}");

		RankingList list = new RankingList();
		list.ranking = ranks;

		// 리스트 요소 확인
		foreach (var rank in ranks)
		{
			Debug.Log($"랭킹: {rank.name} - {rank.score}");
		}

		string path = Path.Combine(Application.dataPath, "ranking.json");
		string json = JsonUtility.ToJson(list, true);

		Debug.Log($"JSON 저장 데이터:\n{json}");

		File.WriteAllText(path, json);
	}

	public void LoadRanking()
	{
		string path = Path.Combine(Application.dataPath, "ranking.json");

		if(!File.Exists(path))
		{
			UpdateRanking();
			return;
		}
		string jsonData = File.ReadAllText(path);
		RankingList list = JsonUtility.FromJson<RankingList>(jsonData);

		if (list != null && list.ranking != null)
		{
			ranks = list.ranking;
		}
	}
	/// <summary>
	/// 열거형 타입을 매개변수로 받아 GameSystem의 해당 필드를 변경해주는 함수
	/// </summary>
	/// <typeparam name="TEnum">입력받을 열거형 타입</typeparam>
	/// <param name="enumvalue">설정할 열거형 값</param>
	/// <exception cref="ArgumentException">지원되지 않는 열거형 타입을 작성할 경우 발생</exception>
	public void SetItemData<TEnum>(TEnum enumvalue) where TEnum : Enum
	{
		Type enumType = typeof(TEnum);
		  
		if (enumType == typeof(WheelType)) wheeltype = (WheelType)(object)enumvalue;

		else if (enumType == typeof(Engine)) engine = (Engine)(object)enumvalue;

		else if (enumType == typeof(Transmission)) transmission = (Transmission)(object)enumvalue;

		else throw new ArgumentException($"지원되지 않는 열거형 타입입니다! \n열거형 타입 : {enumvalue}");

		Debug.Log($"[SetItemData] {enumType.Name} 변경 : {enumvalue}");
	}

	/// <summary>
	/// 설정된 열거형 값을 반환해주는 함수
	/// </summary>
	/// <typeparam name="TEnum">반환할 열거형 타입</typeparam>
	/// <returns>현재 설정된 열거형 값</returns>
	/// <exception cref="ArgumentException">지원되지 않는 열거형 타입을 작성할 경우 발생</exception>
	public TEnum GetItemData<TEnum>() where TEnum : Enum
	{
		Type enumType = typeof(TEnum);

		if (enumType == typeof(WheelType)) return (TEnum)(object)wheeltype;

		else if (enumType == typeof(Engine)) return (TEnum)(object)engine;

		else if (enumType == typeof(Transmission)) return (TEnum)(object)transmission;

		else throw new ArgumentException($"지원되지 않는 열거형 타입입니다! \n열거형 타입 : {nameof(TEnum)}");
	}

}
