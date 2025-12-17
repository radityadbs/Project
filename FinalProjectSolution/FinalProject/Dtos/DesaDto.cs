namespace FinalProject.Dtos;

public class TokenResponse
{
    public int status { get; set; }
    public string pesan { get; set; } = "";
    public string token { get; set; } = "";
}


public class DesaApiResponse
{
    public int total { get; set; }
    public List<DesaDto> rows { get; set; } = new();
}

public class DesaDto
{
    public string id { get; set; } = "";
    public string desa { get; set; } = "";
    public string kecamatan { get; set; } = "";
    public string kabupaten { get; set; } = "";
    public string provinsi { get; set; } = "";
}