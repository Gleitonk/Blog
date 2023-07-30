namespace Blog;

public static class Configuration
{
    public static string JwtKey = "lSgT1XB8r3hdXuvDPVzuQEKt1YMbk17MH76UZCwgYOSXfGbna1jiYaUhmLFikNDc";
    public static string ApitKeyName = "api_key";
    public static string ApitKey = "49[i*BVCC5467r90T391-79JN7824-BDFVAW5684D9870BNV6C43WE54RTCUYgvbhj8165-==-304";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}