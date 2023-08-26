using RisingWin.Library;

public class ExampleCloudStoge : CloudStoge
{
	public override bool FileExists(string filename)
	{
		return true;
	}

	public override void FileReadAsyncProcess()
	{
		AnyCloudReadCallback();
	}

	private void AnyCloudReadCallback()
	{
		ProcessSuccess();
	}

	public override void FileWriteAsyncProcess()
	{
		AnyCloudWriteCallback();
	}

	private void AnyCloudWriteCallback()
	{
		ProcessSuccess();
	}
}
