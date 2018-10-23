public class SoundCalculator
{
	static public int minSource = 40;
	static public int maxSource = 70;

	static public float calc(int midi, SoundCalcMethodType type = SoundCalcMethodType.NORMAL)
	{
		float result = 0;
		switch (type)
		{
			case SoundCalcMethodType.NORMAL:
				result = normalCal(midi);
				break;
			default:
				break;
		}
		return result;
	}
	static float normalCal(int midi)
	{
		if (midi < minSource)
			return 0;
		else if (midi > maxSource)
			return 1;
		else return (float)(midi - minSource) / (maxSource - minSource);
	}
}

public enum SoundCalcMethodType { 
	NORMAL
}