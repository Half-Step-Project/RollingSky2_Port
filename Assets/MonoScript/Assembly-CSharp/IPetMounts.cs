public interface IPetMounts
{
	float UpMountsDuration { get; set; }

	float DownMountsDuration { get; set; }

	void OnUpMounts();

	void OnDownMounts();
}
