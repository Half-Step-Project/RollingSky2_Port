internal interface IPetPathToMove
{
	PathToMoveByPetTrigger.PathToMoveByPetTriggerData PathtoMoveData { get; set; }

	void OnPathToMove();
}
