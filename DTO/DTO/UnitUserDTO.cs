namespace DTO.DTO
{
    public class UnitUserDTO
    {
        public int Id { get; set; }
        public int UserSgqId { get; set; }
        public int UnitId { get; set; }

        public UnitDTO Unit { get; set; }
    }
}
