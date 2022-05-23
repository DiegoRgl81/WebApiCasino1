namespace WebApiCasino.DTOs
{
    public class RifaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ParticipanteDTO> Participante { get; set; }
    }
}
