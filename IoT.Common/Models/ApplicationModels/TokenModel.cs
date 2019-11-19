
namespace IoT.Common.Models
{
    public class TokenModel
    {
        public TokenModel(string accesToken, string refrashToken)
        {
            AccsesToken = accesToken;
            RefrashToken = refrashToken;
        }

        public string AccsesToken { get; private set; }
        public string RefrashToken { get; private set; }

    }
}
