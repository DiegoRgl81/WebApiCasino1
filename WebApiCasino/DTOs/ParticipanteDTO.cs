namespace WebApiCasino.DTOs
{
    public class ParticipanteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int NumeroLoteria { get; set; }

        public List<RifaDTO> Rifas { get; set; }
    }
}
