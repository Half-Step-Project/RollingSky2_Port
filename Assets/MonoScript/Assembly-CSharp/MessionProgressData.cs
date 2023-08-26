public struct MessionProgressData
{
	public int Member_One { get; set; }

	public int Member_Two { get; set; }

	public MessionProgressData(int memberOne, int memberTow)
	{
		Member_One = memberOne;
		Member_Two = memberTow;
	}

	public void Clear()
	{
		Member_One = 0;
		Member_Two = 0;
	}
}
