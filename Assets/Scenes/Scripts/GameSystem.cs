using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSystem
{
    public static GameSystem instance { get; } = new GameSystem();
	private GameSystem() { }

	public float resultTime = 0f;
	public int Money = 0;

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

	public WheelType wheeltype;
	public Engine engine;
	public Transmission transmission;

	private class SaveData
	{
		public string wheeltype;
		public string engine;
		public string transmission;
	}


	public void Init()
	{
		engine = Engine.Normal;
		transmission = Transmission.Normal;
		wheeltype = WheelType.Normal;
	}
	public void SaveItemData()
	{
		SaveData data = new SaveData
		{
			wheeltype = this.wheeltype.ToString(),
			engine = this.engine.ToString(),
			transmission = this.transmission.ToString()
		};

		string json = JsonUtility.ToJson(data, true);
		string path = Path.Combine(Application.dataPath, "playerData.json");

		File.WriteAllText(path, json);
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
